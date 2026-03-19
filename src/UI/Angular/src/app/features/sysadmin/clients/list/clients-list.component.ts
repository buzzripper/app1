import { NgClass, NgTemplateOutlet } from '@angular/common';
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
    UntypedFormControl,
    UntypedFormGroup,
    Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { fuseAnimations } from '@fuse/animations';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { ClientService } from '@app/core/services/app/client.service';
import { ClientDto } from '@app/core/services/app/dto';
import { CreateClientReq } from '@app/core/services/app/req';
import { Subject, firstValueFrom, map, takeUntil } from 'rxjs';

@Component({
    selector: 'clients-list',
    templateUrl: './clients-list.component.html',
    styles: [
        `
            .clients-grid {
                grid-template-columns: auto 112px 150px;

                @screen sm {
                    grid-template-columns: auto 200px 200px 72px;
                }

                @screen md {
                    grid-template-columns: auto 200px 500px 72px;
                }
            }
        `,
    ],
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush,
    animations: fuseAnimations,
    imports: [
        NgClass,
        NgTemplateOutlet,
        FormsModule,
        ReactiveFormsModule,
        MatButtonModule,
        MatFormFieldModule,
        MatIconModule,
        MatInputModule,
        MatProgressBarModule,
    ],
})
export class ClientsListComponent implements OnInit, OnDestroy {
    clients: ClientDto[] = [];
    filteredClients: ClientDto[] = [];
    selectedClient: ClientDto | null = null;
    selectedClientForm: UntypedFormGroup;
    searchInputControl: UntypedFormControl = new UntypedFormControl();
    flashMessage: 'success' | 'error' | null = null;
    isLoading: boolean = false;
    private _unsubscribeAll: Subject<any> = new Subject<any>();

    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        private _formBuilder: UntypedFormBuilder,
        private _clientService: ClientService,
        private _fuseConfirmationService: FuseConfirmationService
    ) {}

    async ngOnInit(): Promise<void> {
        this.selectedClientForm = this._formBuilder.group({
            id: [''],
            key: ['', [Validators.required]],
            name: ['', [Validators.required]],
            baseUrl: ['', [Validators.required]],
        });

        // Load initial data
        const clients = await firstValueFrom(
            this._clientService
                .getAllClients({ sortBy: 'name', sortDesc: false })
                .pipe(
                    map((response: any) => Array.isArray(response) ? response : response?.data ?? []),
                ),
        );
        this.clients = clients;
        this.filteredClients = clients;
        this._changeDetectorRef.markForCheck();

        // Search filter
        this.searchInputControl.valueChanges
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((query: string) => {
                const lowerQuery = (query || '').toLowerCase();
                this.filteredClients = this.clients.filter(
                    (c) =>
                        c.name.toLowerCase().includes(lowerQuery) ||
                        c.key.toLowerCase().includes(lowerQuery)
                );
                this.closeDetails();
                this._changeDetectorRef.markForCheck();
            });
    }

    ngOnDestroy(): void {
        this._unsubscribeAll.next(null);
        this._unsubscribeAll.complete();
    }

    toggleDetails(clientId: string): void {
        if (this.selectedClient && this.selectedClient.id === clientId) {
            this.closeDetails();
            return;
        }

        this.isLoading = true;
        this._clientService.getClientById(clientId).subscribe((client) => {
            this.selectedClient = client;
            this.selectedClientForm.patchValue(client);
            this.isLoading = false;
            this._changeDetectorRef.markForCheck();
        });
    }

    closeDetails(): void {
        this.selectedClient = null;
    }

    createClient(): void {
        const newId = crypto.randomUUID().toString();
        const newClient: CreateClientReq = {
            id: newId,
            rowVersion: new Uint8Array(),
            key: `new-client-${newId.substring(0, 8)}`,
            name: 'New Client',
            baseUrl: 'https://',
        };

        this._clientService
            .createClient(newClient)
            .subscribe((rowVersion) => {
                this._clientService.getClientById(newId).subscribe((client) => {
                    this.selectedClient = client;
                    this.selectedClientForm.patchValue(client);
                    this.refreshList();
                    this._changeDetectorRef.markForCheck();
                });
            });
    }

    updateSelectedClient(): void {
        const formValue = this.selectedClientForm.getRawValue();
        this._clientService
            .updateClient({
                id: formValue.id,
                rowVersion: null,
                key: formValue.key,
                name: formValue.name,
                baseUrl: formValue.baseUrl,
            })
            .subscribe({
                next: () => {
                    this.refreshList();
                    this.showFlashMessage('success');
                },
                error: () => {
                    this.showFlashMessage('error');
                },
            });
    }

    deleteSelectedClient(): void {
        const confirmation = this._fuseConfirmationService.open({
            title: 'Delete client',
            message:
                'Are you sure you want to remove this client? This action cannot be undone!',
            actions: {
                confirm: { label: 'Delete' },
            },
        });

        confirmation.afterClosed().subscribe((result) => {
            if (result === 'confirmed') {
                const client = this.selectedClientForm.getRawValue();
                this._clientService.delete(client.id).subscribe(() => {
                    this.closeDetails();
                    this.refreshList();
                });
            }
        });
    }

    private refreshList(): void {
        this._clientService
            .getAllClients({ sortBy: 'name', sortDesc: false })
            .pipe(
                map((response: any) => Array.isArray(response) ? response : response?.data ?? []),
            )
            .subscribe((clients: ClientDto[]) => {
                this.clients = clients;
                const query = (this.searchInputControl.value || '').toLowerCase();
                this.filteredClients = clients.filter(
                    (c) =>
                        c.name.toLowerCase().includes(query) ||
                        c.key.toLowerCase().includes(query)
                );
                this._changeDetectorRef.markForCheck();
            });
    }

    showFlashMessage(type: 'success' | 'error'): void {
        this.flashMessage = type;
        this._changeDetectorRef.markForCheck();
        setTimeout(() => {
            this.flashMessage = null;
            this._changeDetectorRef.markForCheck();
        }, 3000);
    }

    trackByFn(index: number, item: any): any {
        return item.id || index;
    }
}
