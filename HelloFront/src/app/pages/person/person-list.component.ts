import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { PersonService } from '../../services/person.service';
import { Person } from '../../models/person';

@Component({
  selector: 'app-person-list',
  standalone: true,
  imports: [CommonModule, RouterModule, MatCardModule, MatButtonModule, MatIconModule, MatTableModule],
  templateUrl: './person-list.component.html',
  styleUrls: ['./person-list.component.css']
})
export class PersonListComponent {
  private svc = inject(PersonService);
  private router = inject(Router);

  persons = signal<Person[]>([]);
  displayed = ['id', 'name', 'email', 'actions'];

  ngOnInit() { this.load(); }
  load() { this.svc.getAll().subscribe(p => this.persons.set(p)); }

  edit(p: Person) { this.router.navigate(['/persons', p.id]); }
  remove(p: Person) {
    if (!p.id) return;
    if (confirm(`¿Eliminar a ${p.firstName} ${p.lastName}?`)) {
      this.svc.delete(p.id).subscribe({ next: () => this.load() });
    }
  }
}
