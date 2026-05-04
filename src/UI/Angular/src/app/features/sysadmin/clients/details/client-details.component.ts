import { TextFieldModule } from '@angular/cdk/text-field';
import {
    ChangeDetectionStrategy,
    ChangeDetectorRef,
    Component,
    OnDestroy,
    OnInit,
    ViewEncapsulation,
} from '@angular/core';
import {
    FormsModule,
    ReactiveFormsModule,
    UntypedFormBuilder,
    UntypedFormGroup,
    Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatOptionModule } from '@angular/material/core';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSelectModule } from '@angular/material/select';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ClientService } from '@app/core/services/app/client.service';
import { ClientAuthService } from '@app/core/services/app/client-auth.service';
import { ClientDto } from '@app/core/services/app/dto';
import { AuthMode, TenantDto } from '@app/core/services/app/dto';
import { CreateClientReq } from '@app/core/services/app/req';
import { OidcAppDto } from '@app/core/services/auth/dto';
import { OidcAppService } from '@app/core/services/auth/oidc-app.service';
import { Subject, firstValueFrom, takeUntil } from 'rxjs';

interface SectionMessage {
    type: 'success' | 'error';
    text: string;
}

@Component({
    selector: 'client-details',
    templateUrl: './client-details.component.html',
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [
        RouterLink,
        FormsModule,
        ReactiveFormsModule,
        MatButtonModule,
        MatExpansionModule,
        MatFormFieldModule,
        MatIconModule,
        MatInputModule,
        MatOptionModule,
        MatProgressBarModule,
        MatSelectModule,
        MatSlideToggleModule,
        TextFieldModule,
    ],
})
export class ClientDetailsComponent implements OnInit, OnDestroy {
    readonly authModes = Object.values(AuthMode);
    readonly authModeEnum = AuthMode;

    clientForm: UntypedFormGroup;
    tenantForm: UntypedFormGroup;
    oidcAppForm: UntypedFormGroup;

    client: ClientDto | null = null;
    isNew: boolean = false;
    hasTenant: boolean = false;
    hasOidcApp: boolean = false;
    isLoading: boolean = false;
    isLoadingTenantSection: boolean = false;
    isLoadingOidcAppSection: boolean = false;
    isSavingClient: boolean = false;
    isSavingTenant: boolean = false;
    isSavingOidcApp: boolean = false;
    clientMessage: SectionMessage | null = null;
    tenantMessage: SectionMessage | null = null;
    oidcAppMessage: SectionMessage | null = null;

    private _unsubscribeAll: Subject<void> = new Subject<void>();

    get selectedAuthMode(): AuthMode {
        return this.toAuthMode(this.tenantForm?.get('authMode')?.value);
    }

    constructor(
        private _activatedRoute: ActivatedRoute,
        private _changeDetectorRef: ChangeDetectorRef,
        private _clientService: ClientService,
        private _clientAuthService: ClientAuthService,
        private _formBuilder: UntypedFormBuilder,
        private _fuseConfirmationService: FuseConfirmationService,
        private _oidcAppService: OidcAppService,
        private _router: Router
    ) {}

    ngOnInit(): void {
        this.clientForm = this._formBuilder.group({
            id: [{ value: '', disabled: true }],
            key: ['', [Validators.required]],
            name: ['', [Validators.required]],
            baseUrl: ['', [Validators.required]],
        });

        this.tenantForm = this._formBuilder.group({
            id: [{ value: '', disabled: true }],
            name: ['', [Validators.required]],
            slug: ['', [Validators.required]],
            authMode: [AuthMode.Local, [Validators.required]],
            externalAuthority: [''],
            externalClientId: [''],
            externalClientSecret: [''],
            adDcHost: [''],
            adDomain: [''],
            adLdapPort: [null],
            adBaseDn: [''],
            isActive: [true],
            createdAt: ['', [Validators.required]],
        });

        this.oidcAppForm = this._formBuilder.group({
            id: [{ value: '', disabled: true }],
            applicationType: [''],
            clientId: ['', [Validators.required]],
            clientSecret: [''],
            displayName: [''],
            redirectUrisText: [''],
            postLogoutRedirectUrisText: [''],
        });

        this.tenantForm
            .get('authMode')
            ?.valueChanges.pipe(takeUntil(this._unsubscribeAll))
            .subscribe(() => {
                this._changeDetectorRef.markForCheck();
            });

        this._activatedRoute.paramMap
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((paramMap) => {
                const id = paramMap.get('id');
                if (id === 'new') {
                    this.initNewClient();
                } else if (id) {
                    void this.loadClientDetails(id);
                }
            });
    }

    ngOnDestroy(): void {
        this._unsubscribeAll.next();
        this._unsubscribeAll.complete();
    }

    async saveClient(): Promise<void> {
        if (this.clientForm.invalid) {
            this.clientForm.markAllAsTouched();
            return;
        }

        if (this.isNew) {
            await this.createClient();
        } else {
            await this.updateClient();
        }
    }

    async deleteClient(): Promise<void> {
        if (!this.client) {
            return;
        }

        const confirmed = await this.confirmAction(
            'Delete client',
            'Are you sure you want to remove this client? This action cannot be undone!'
        );

        if (!confirmed) {
            return;
        }

        try {
            await this._clientService.delete(this.client.id);
            await this._router.navigate(['../'], {
                relativeTo: this._activatedRoute,
            });
        } catch {
            this.showMessage('client', 'error', 'Unable to delete client.');
            this._changeDetectorRef.markForCheck();
        }
    }

    async saveTenant(): Promise<void> {
        if (this.tenantForm.invalid || !this.client) {
            this.tenantForm.markAllAsTouched();
            return;
        }

        const confirmed = await this.confirmAction(
            'Save OIDC security',
            'Save changes to the OIDC security configuration?'
        );

        if (!confirmed) {
            return;
        }

        const formValue = this.tenantForm.getRawValue();
        const request = {
            id: formValue.id,
            name: formValue.name,
            slug: formValue.slug,
            authMode: formValue.authMode,
            externalAuthority: this.normalizeString(formValue.externalAuthority),
            externalClientId: this.normalizeString(formValue.externalClientId),
            externalClientSecret: this.normalizeString(formValue.externalClientSecret),
            adDcHost: this.normalizeString(formValue.adDcHost),
            adDomain: this.normalizeString(formValue.adDomain),
            adLdapPort: this.normalizeNumber(formValue.adLdapPort),
            adBaseDn: this.normalizeString(formValue.adBaseDn),
            isActive: !!formValue.isActive,
            createdAt: this.toIsoString(formValue.createdAt),
        };

        this.isSavingTenant = true;
        this.tenantMessage = null;
        this._changeDetectorRef.markForCheck();

        try {
            if (this.hasTenant) {
                await this._clientAuthService.updateTenant(request);
            } else {
                await this._clientAuthService.createTenant(request);
            }

            this.showMessage(
                'tenant',
                'success',
                this.hasTenant ? 'Tenant saved.' : 'Tenant created.'
            );
            await this.loadTenantSection();
        } catch {
            this.showMessage('tenant', 'error', 'Unable to save tenant.');
        } finally {
            this.isSavingTenant = false;
            this._changeDetectorRef.markForCheck();
        }
    }

    async deleteTenant(): Promise<void> {
        if (!this.client || !this.hasTenant) {
            return;
        }

        const id = this.tenantForm.getRawValue().id;
        const confirmed = await this.confirmAction(
            'Delete tenant',
            'Are you sure you want to remove this tenant configuration?'
        );

        if (!confirmed) {
            return;
        }

        try {
            await this._clientAuthService.deleteTenant(id);
            this.hasTenant = false;
            this.patchTenantDefaults(this.client);
            this.showMessage('tenant', 'success', 'Tenant deleted.');
        } catch {
            this.showMessage('tenant', 'error', 'Unable to delete tenant.');
        } finally {
            this._changeDetectorRef.markForCheck();
        }
    }

    async saveOidcApp(): Promise<void> {
        if (this.oidcAppForm.invalid || !this.client) {
            this.oidcAppForm.markAllAsTouched();
            return;
        }

        const confirmed = await this.confirmAction(
            'Save app registrations',
            'Save changes to the app registrations configuration?'
        );

        if (!confirmed) {
            return;
        }

        const formValue = this.oidcAppForm.getRawValue();
        const request = {
            id: formValue.id,
            applicationType: this.normalizeString(formValue.applicationType),
            clientId: formValue.clientId,
            clientSecret: this.normalizeString(formValue.clientSecret),
            displayName: this.normalizeString(formValue.displayName),
            redirectUris: this.parseUriList(formValue.redirectUrisText),
            postLogoutRedirectUris: this.parseUriList(
                formValue.postLogoutRedirectUrisText
            ),
        };

        this.isSavingOidcApp = true;
        this.oidcAppMessage = null;
        this._changeDetectorRef.markForCheck();

        try {
            if (this.hasOidcApp) {
                await this._oidcAppService.update(request);
            } else {
                await this._oidcAppService.create(request);
            }

            this.showMessage(
                'oidcApp',
                'success',
                this.hasOidcApp
                    ? 'OpenIddict application saved.'
                    : 'OpenIddict application created.'
            );
            await this.loadOidcAppSection();
        } catch {
            this.showMessage(
                'oidcApp',
                'error',
                'Unable to save OpenIddict application.'
            );
        } finally {
            this.isSavingOidcApp = false;
            this._changeDetectorRef.markForCheck();
        }
    }

    async deleteOidcApp(): Promise<void> {
        if (!this.client || !this.hasOidcApp) {
            return;
        }

        const id = this.oidcAppForm.getRawValue().id;
        const confirmed = await this.confirmAction(
            'Delete OpenIddict application',
            'Are you sure you want to remove this OpenIddict application configuration?'
        );

        if (!confirmed) {
            return;
        }

        try {
            await this._oidcAppService.delete(id);
            this.hasOidcApp = false;
            this.patchOidcAppDefaults(this.client);
            this.showMessage('oidcApp', 'success', 'OpenIddict application deleted.');
        } catch {
            this.showMessage(
                'oidcApp',
                'error',
                'Unable to delete OpenIddict application.'
            );
        } finally {
            this._changeDetectorRef.markForCheck();
        }
    }

    async loadTenantSection(): Promise<void> {
        if (!this.client) {
            return;
        }

        this.isLoadingTenantSection = true;
        this.tenantMessage = null;
        this._changeDetectorRef.markForCheck();

        try {
            const tenant = await this._clientAuthService.getTenantById(this.client.extAuthId);

            if (tenant) {
                this.hasTenant = true;
                this.patchTenantForm(tenant);
            } else {
                this.hasTenant = false;
                this.patchTenantDefaults(this.client);
            }
        } catch {
            this.showMessage('tenant', 'error', 'Unable to load OIDC security.');
        } finally {
            this.isLoadingTenantSection = false;
            this._changeDetectorRef.markForCheck();
        }
    }

    async loadOidcAppSection(): Promise<void> {
        if (!this.client) {
            return;
        }

        this.isLoadingOidcAppSection = true;
        this.oidcAppMessage = null;
        this._changeDetectorRef.markForCheck();

        try {
            const oidcApp = await this._oidcAppService.getByClientId(this.client.key);

            if (oidcApp) {
                this.hasOidcApp = true;
                this.patchOidcAppForm(oidcApp);
            } else {
                this.hasOidcApp = false;
                this.patchOidcAppDefaults(this.client);
            }
        } catch {
            this.showMessage('oidcApp', 'error', 'Unable to load app registrations.');
        } finally {
            this.isLoadingOidcAppSection = false;
            this._changeDetectorRef.markForCheck();
        }
    }

    private initNewClient(): void {
        const newId = crypto.randomUUID().toString();
        this.isNew = true;
        this.client = null;
        this.hasTenant = false;
        this.hasOidcApp = false;
        this.clientForm.reset({
            id: newId,
            key: '',
            name: '',
            baseUrl: 'https://',
        });
        this._changeDetectorRef.markForCheck();
    }

    private async createClient(): Promise<void> {
        const formValue = this.clientForm.getRawValue();
        const request: CreateClientReq = {
            id: formValue.id,
            rowVersion: new Uint8Array(),
            key: formValue.key,
            name: formValue.name,
            baseUrl: formValue.baseUrl,
        };

        this.isSavingClient = true;
        this.clientMessage = null;
        this._changeDetectorRef.markForCheck();

        try {
            await this._clientService.createClient(request);
            this.isNew = false;
            this.showMessage('client', 'success', 'Client created.');
            await this._router.navigate(['../', formValue.id], {
                relativeTo: this._activatedRoute,
                replaceUrl: true,
            });
        } catch {
            this.showMessage('client', 'error', 'Unable to create client.');
        } finally {
            this.isSavingClient = false;
            this._changeDetectorRef.markForCheck();
        }
    }

    private async updateClient(): Promise<void> {
        if (!this.client) {
            return;
        }

        const confirmed = await this.confirmAction(
            'Save client',
            'Save changes to this client information?'
        );

        if (!confirmed) {
            return;
        }

        const formValue = this.clientForm.getRawValue();
        this.isSavingClient = true;
        this.clientMessage = null;
        this._changeDetectorRef.markForCheck();

        try {
            await this._clientService.updateClient({
                id: formValue.id,
                rowVersion: null,
                key: formValue.key,
                name: formValue.name,
                baseUrl: formValue.baseUrl,
            });

            this.showMessage('client', 'success', 'Client saved.');
            await this.loadClientDetails(this.client.id);
        } catch {
            this.showMessage('client', 'error', 'Unable to save client.');
        } finally {
            this.isSavingClient = false;
            this._changeDetectorRef.markForCheck();
        }
    }

    private async loadClientDetails(id: string): Promise<void> {
        this.isLoading = true;
        this.isNew = false;
        this.clientMessage = null;
        this.tenantMessage = null;
        this.oidcAppMessage = null;
        this._changeDetectorRef.markForCheck();

        try {
            const client = await this._clientService.getClientById(id);
            this.client = client;
            this.clientForm.patchValue(client);
            this.hasTenant = false;
            this.hasOidcApp = false;
            this.patchTenantDefaults(client);
            this.patchOidcAppDefaults(client);
        } catch {
            await this._router.navigate(['../'], {
                relativeTo: this._activatedRoute,
            });
        } finally {
            this.isLoading = false;
            this._changeDetectorRef.markForCheck();
        }
    }

    private patchTenantForm(tenant: TenantDto): void {
        this.tenantForm.reset({
            id: tenant.id,
            name: tenant.name,
            slug: tenant.slug,
            authMode: this.toAuthMode(tenant.authMode),
            externalAuthority: tenant.externalAuthority ?? '',
            externalClientId: tenant.externalClientId ?? '',
            externalClientSecret: tenant.externalClientSecret ?? '',
            adDcHost: tenant.adDcHost ?? '',
            adDomain: tenant.adDomain ?? '',
            adLdapPort: tenant.adLdapPort,
            adBaseDn: tenant.adBaseDn ?? '',
            isActive: tenant.isActive,
            createdAt: this.toDateTimeLocal(tenant.createdAt),
        });
    }

    private patchTenantDefaults(client: ClientDto): void {
        this.tenantForm.reset({
            id: client.id,
            name: client.name,
            slug: client.key,
            authMode: AuthMode.Local,
            externalAuthority: '',
            externalClientId: '',
            externalClientSecret: '',
            adDcHost: '',
            adDomain: '',
            adLdapPort: null,
            adBaseDn: '',
            isActive: true,
            createdAt: this.toDateTimeLocal(new Date().toISOString()),
        });
    }

    private patchOidcAppForm(oidcApp: OidcAppDto): void {
        this.oidcAppForm.reset({
            id: oidcApp.id,
            applicationType: oidcApp.applicationType ?? '',
            clientId: oidcApp.clientId,
            clientSecret: oidcApp.clientSecret ?? '',
            displayName: oidcApp.displayName ?? '',
            redirectUrisText: this.formatUriList(oidcApp.redirectUris),
            postLogoutRedirectUrisText: this.formatUriList(
                oidcApp.postLogoutRedirectUris
            ),
        });
    }

    private patchOidcAppDefaults(client: ClientDto): void {
        this.oidcAppForm.reset({
            id: '',
            applicationType: '',
            clientId: client.key,
            clientSecret: '',
            displayName: client.name,
            redirectUrisText: client.baseUrl ? `${client.baseUrl}/signin-oidc` : '',
            postLogoutRedirectUrisText: client.baseUrl
                ? `${client.baseUrl}/signout-callback-oidc`
                : '',
        });
    }

    private async confirmAction(title: string, message: string): Promise<boolean> {
        const confirmation = this._fuseConfirmationService.open({
            title,
            message,
            actions: {
                confirm: {
                    label: 'Confirm',
                },
            },
        });

        return (await firstValueFrom(confirmation.afterClosed())) === 'confirmed';
    }

    private parseUriList(value: string): string[] {
        return (value ?? '')
            .split(/\r?\n/)
            .map((item) => item.trim())
            .filter((item) => item.length > 0);
    }

    private formatUriList(value: string[]): string {
        return value?.join('\n') ?? '';
    }

    private toDateTimeLocal(value: string): string {
        const date = new Date(value);
        return Number.isNaN(date.getTime())
            ? ''
            : new Date(date.getTime() - date.getTimezoneOffset() * 60000)
                  .toISOString()
                  .slice(0, 16);
    }

    private toIsoString(value: string): string {
        return value ? new Date(value).toISOString() : new Date().toISOString();
    }

    private normalizeString(value: string | null | undefined): string | null {
        const trimmedValue = value?.trim();
        return trimmedValue ? trimmedValue : null;
    }

    private normalizeNumber(value: number | string | null | undefined): number | null {
        if (value === null || value === undefined || value === '') {
            return null;
        }

        const parsed = Number(value);
        return Number.isFinite(parsed) ? parsed : null;
    }

    private toAuthMode(value: AuthMode | string | number | null | undefined): AuthMode {
        if (value === AuthMode.AD || value === 1 || value === '1') {
            return AuthMode.AD;
        }

        if (
            value === AuthMode.ExternalOidc ||
            value === 2 ||
            value === '2'
        ) {
            return AuthMode.ExternalOidc;
        }

        return AuthMode.Local;
    }

    private showMessage(
        target: 'client' | 'tenant' | 'oidcApp',
        type: 'success' | 'error',
        text: string
    ): void {
        const message = { type, text };

        if (target === 'client') {
            this.clientMessage = message;
        }

        if (target === 'tenant') {
            this.tenantMessage = message;
        }

        if (target === 'oidcApp') {
            this.oidcAppMessage = message;
        }

        this._changeDetectorRef.markForCheck();

        setTimeout(() => {
            if (target === 'client' && this.clientMessage === message) {
                this.clientMessage = null;
            }

            if (target === 'tenant' && this.tenantMessage === message) {
                this.tenantMessage = null;
            }

            if (target === 'oidcApp' && this.oidcAppMessage === message) {
                this.oidcAppMessage = null;
            }

            this._changeDetectorRef.markForCheck();
        }, 3000);
    }
}
