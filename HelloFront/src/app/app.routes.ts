import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'persons', loadComponent: () => import('./pages/person/person-list.component').then(m => m.PersonListComponent) },
  { path: 'persons/new', loadComponent: () => import('./pages/person/person-form.component').then(m => m.PersonFormComponent) },
  { path: 'persons/:id', loadComponent: () => import('./pages/person/person-form.component').then(m => m.PersonFormComponent) },
  { path: 'items', loadComponent: () => import('./pages/item/item-list.component').then(m => m.ItemListComponent) },
  { path: 'items/new', loadComponent: () => import('./pages/item/item-form.component').then(m => m.ItemFormComponent) },
  { path: 'items/:id', loadComponent: () => import('./pages/item/item-form.component').then(m => m.ItemFormComponent) },
  { path: 'orders/create', loadComponent: () => import('./pages/order/order-create.component').then(m => m.OrderCreateComponent) },
  { path: 'orders', loadComponent: () => import('./pages/order/order-list.component').then(m => m.OrderListComponent) },
  { path: 'orders/:id', loadComponent: () => import('./pages/order/order-detail.component').then(m => m.OrderDetailComponent) },

];
