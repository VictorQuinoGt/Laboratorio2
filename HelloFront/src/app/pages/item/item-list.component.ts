import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { ItemService } from '../../services/item.service';
import { Item } from '../../models/item';

@Component({
  selector: 'app-item-list',
  standalone: true,
  imports: [CommonModule, RouterModule, MatCardModule, MatButtonModule, MatIconModule, MatTableModule],
  templateUrl: './item-list.component.html',
  styleUrls: ['./item-list.component.css']
})
export class ItemListComponent {
  private svc = inject(ItemService);
  private router = inject(Router);

  items = signal<Item[]>([]);
  displayed = ['id', 'name', 'price', 'actions'];

  ngOnInit() { this.load(); }
  load() { this.svc.getAll().subscribe(x => this.items.set(x)); }

  edit(it: Item) { this.router.navigate(['/items', it.id]); }
  remove(it: Item) {
    if (!it.id) return;
    if (confirm(`¿Eliminar item "${it.name}"?`)) {
      this.svc.delete(it.id).subscribe({ next: () => this.load() });
    }
  }
}
