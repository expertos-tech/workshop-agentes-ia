import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Product {
  id: number;
  code: string;
  productName: string;
  brands?: string;
  categories?: string;
}

@Injectable({ providedIn: 'root' })
export class ProductService {
  private apiUrl = 'http://localhost:5037/api/products';

  constructor(private http: HttpClient) {}

  getProducts(search?: string): Observable<Product[]> {
    const url = search ? `${this.apiUrl}?search=${encodeURIComponent(search)}` : this.apiUrl;
    return this.http.get<Product[]>(url);
  }

  triggerFetch(query: string = 'chocolate'): Observable<any> {
    return this.http.post(`http://localhost:5037/api/products/fetch?query=${encodeURIComponent(query)}`, {});
  }
}
