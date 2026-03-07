import { NgClass } from '@angular/common';
import {
    ChangeDetectionStrategy,
    Component,
    OnInit,
    ViewEncapsulation,
    signal,
} from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { SystemTileStatus } from './system.types';
import { AuthSystemService } from 'app/core/services/auth/system.service';
import { AppSystemService } from 'app/core/services/app/system.service';
import { PortalSystemService } from 'app/core/services/portal/system.service';
import {StatusLevel} from 'app/core/services/common/dtos';

@Component({
    selector: 'system',
    templateUrl: './system.component.html',
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [MatIconModule, MatButtonModule, NgClass],
})
export class SystemComponent implements OnInit {
    tiles = signal<SystemTileStatus[]>([
        {
            module: 'auth',
            title: 'Auth',
            pingStatus: 'unknown',
            healthStatus: 'unknown',
        },
        {
            module: 'app',
            title: 'App',
            pingStatus: 'unknown',
            healthStatus: 'unknown',
        },
        {
            module: 'portal',
            title: 'Portal',
            pingStatus: 'unknown',
            healthStatus: 'unknown',
        },
        {
            module: 'adagent',
            title: 'AD Agent',
            pingStatus: 'unknown',
            healthStatus: 'unknown',
        }
    ]);

    constructor(
        private _authSystemService: AuthSystemService,
        private _appSystemService: AppSystemService,
        private _portalSystemService: PortalSystemService
    ) {}

    ngOnInit(): void {}

    callPing(module: 'auth' | 'app' | 'portal' | 'adagent'): void {
        const service =
            module === 'auth'
                ? this._authSystemService
                : module === 'app'
                ? this._appSystemService
                : this._portalSystemService;

        service.ping().subscribe({
            next: (response) => {
                this.updateTileStatus(module, 'ping', 'success', response.module);
            },
            error: (error) => {
                this.updateTileStatus(
                    module,
                    'ping',
                    'error',
                    error.message || 'Ping failed'
                );
            },
        });
    }

    callHealth(module: 'auth' | 'app' | 'portal' | 'adagent'): void {
        const service =
            module === 'auth'
                ? this._authSystemService
                : module === 'app'
                ? this._appSystemService
                : this._portalSystemService;

        service.health().subscribe({
            next: (response) => {
                this.updateTileStatus(
                    module,
                    'health',
                    StatusLevel[response.status],
                    response.message
                );
            },
            error: (error) => {
                this.updateTileStatus(
                    module,
                    'health',
                    'error',
                    error.message || 'Health check failed'
                );

            },
        });
    }

    private updateTileStatus(
        module: 'auth' | 'app' | 'portal' | 'adagent',
        type: 'ping' | 'health',
        status: string,
        message?: string
    ): void {
        this.tiles.update((tiles) =>
            tiles.map((tile) => {
                if (tile.module === module) {
                    if (type === 'ping') {
                        return {
                            ...tile,
                            pingStatus: status,
                            pingMessage: message,
                        };
                    } else {
                        return {
                            ...tile,
                            healthStatus: status,
                            healthMessage: message,
                        };
                    }
                }
                return tile;
            })
        );
    }
}
