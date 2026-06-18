import { booleanAttribute, Component, Input, signal } from '@angular/core';
import { CommonModule } from '@angular/common';

enum BlockView {
    PREVIEW,
    CODE
}

@Component({
    selector: 'block-viewer',
    standalone: true,
    imports: [CommonModule],
    templateUrl: './blockviewer.html'
})
export class BlockViewer {
    @Input() header!: string;

    @Input() code!: string;

    @Input() containerClass!: string;

    @Input() previewStyle!: object;

    @Input({ transform: booleanAttribute }) free: boolean = true;

    @Input({ transform: booleanAttribute }) new: boolean = false;

    BlockView = BlockView;

    blockView = signal<BlockView>(BlockView.PREVIEW);

    codeCopyLoading = signal(false);

    codeCopied = signal(false);

    activateView(event: Event, blockView: BlockView) {
        this.blockView.set(blockView);
        event.preventDefault();
    }

    async copyCode(event: Event) {
        this.codeCopyLoading.set(true);
        event.preventDefault();

        await navigator.clipboard.writeText(this.code);

        this.codeCopyLoading.set(false);
        this.codeCopied.set(true);

        setTimeout(() => {
            this.codeCopied.set(false);
        }, 2000);
    }
}
