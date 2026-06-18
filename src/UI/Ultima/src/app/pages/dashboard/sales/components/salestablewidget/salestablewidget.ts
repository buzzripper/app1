import { Component, signal } from '@angular/core';
import { ProductService } from '@/app/pages/service/product.service';
import { Product } from '@/app/types/product';
import { CommonModule } from '@angular/common';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';

@Component({
    selector: 'sales-table-widget',
    standalone: true,
    imports: [CommonModule, TableModule, ButtonModule, RippleModule],
    templateUrl: './salestablewidget.html',
    providers: [ProductService]
})
export class SalesTableWidget {
    products = signal<Product[]>([]);

    constructor(public productService: ProductService) {
        this.productService.getProducts().then((data: any) => {
            this.products.set(data);
        });
    }
}
