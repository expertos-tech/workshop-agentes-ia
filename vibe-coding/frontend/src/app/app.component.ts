import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Product, ProductService } from './product.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  products: Product[] = [];
  searchTerm = '';
  fetchQuery = 'chocolate';
  loading = false;
  message = '';

  constructor(private productService: ProductService) {}

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.loading = true;
    this.productService.getProducts(this.searchTerm).subscribe({
      next: (data) => { this.products = data; this.loading = false; },
      error: () => { this.message = 'Erro ao carregar produtos'; this.loading = false; }
    });
  }

  triggerEtl(): void {
    this.loading = true;
    this.message = 'Executando ETL...';
    this.productService.triggerFetch(this.fetchQuery).subscribe({
      next: () => { this.message = 'ETL concluído!'; this.loadProducts(); },
      error: () => { this.message = 'Erro no ETL'; this.loading = false; }
    });
  }
}
