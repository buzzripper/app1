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
    UntypedFormControl,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { fuseAnimations } from '@fuse/animations';
import { ClientService } from '@app/core/services/app/client.service';
import { ClientDto } from '@app/core/services/app/dto';
import { Subject, takeUntil } from 'rxjs';

@Component({
    selector: 'clients-list',
    templateUrl: './clients-list.component.html',
    styles: [
        `
            .clients-grid {
                grid-template-columns: minmax(0, 1fr);

                @screen sm {
                    grid-template-columns: minmax(0, 2fr) minmax(0, 1fr)
                        minmax(0, 2fr);
                }

                @screen md {
                    grid-template-columns: minmax(0, 2fr) minmax(0, 1fr)
                        minmax(0, 3fr);
                }
            }
        `,
    ],
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush,
    animations: fuseAnimations,
    imports: [
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
    searchInputControl: UntypedFormControl = new UntypedFormControl();
    isLoading: boolean = false;
    private _unsubscribeAll: Subject<any> = new Subject<any>();

    constructor(
        private _activatedRoute: ActivatedRoute,
        private _changeDetectorRef: ChangeDetectorRef,
        private _clientService: ClientService,
        private _router: Router
    ) {}

    async ngOnInit(): Promise<void> {
        await this.refreshList();

        this.searchInputControl.valueChanges
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((query: string) => {
                const lowerQuery = (query || '').toLowerCase();
                this.filteredClients = this.clients.filter(
                    (c) =>
                        c.name.toLowerCase().includes(lowerQuery) ||
                        c.key.toLowerCase().includes(lowerQuery)
                );
                this._changeDetectorRef.markForCheck();
            });
    }

    ngOnDestroy(): void {
        this._unsubscribeAll.next(null);
        this._unsubscribeAll.complete();
    }

    openClient(clientId: string): void {
        void this._router.navigate([clientId], {
            relativeTo: this._activatedRoute,
        });
    }

    createClient(): void {
        void this._router.navigate(['new'], {
            relativeTo: this._activatedRoute,
        });
    }

    private async refreshList(): Promise<void> {
        this.isLoading = true;
        this._changeDetectorRef.markForCheck();

        try {
            const clients = await this._clientService.getAllClients({ sortBy: 'name', sortDesc: false });
            this.clients = clients;
            const query = (this.searchInputControl.value || '').toLowerCase();
            this.filteredClients = clients.filter(
                (c) =>
                    c.name.toLowerCase().includes(query) ||
                    c.key.toLowerCase().includes(query)
            );
        } finally {
            this.isLoading = false;
            this._changeDetectorRef.markForCheck();
        }
    }

    trackByFn(index: number, item: any): any {
        return item.id || index;
    }
}
