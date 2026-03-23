import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

interface Bilforsikringstilbud {
  regnummer: string;
  merke: string;
  modell: string;
  arsmodell: number;
  forsikringspris: number;
  dekning: string;
  bonus: number;
  egenandel: number;
  dekningsalternativer: Dekningsalternativ[];
}

interface Dekningsalternativ {
  navn: string;
  pris: number;
  beskrivelse: string;
}

interface ContactRequest {
  regnummer: string;
  telefonnummer: string;
  epost: string;
}

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  private http = inject(HttpClient);
  private apiUrl = '/api';

  regInput = signal('');
  result = signal<Bilforsikringstilbud | null>(null);
  isLoading = signal(false);
  hasSearched = signal(false);
  error = signal<string | null>(null);

  // Contact form
  phoneNumber = signal('');
  email = signal('');
  isSubmittingContact = signal(false);
  contactSubmitted = signal(false);

  // Offer details
  bonusOptions = [0, 10, 20, 30, 40, 50, 60, 70, 75];
  selectedBonus = signal(70);

  onSubmit(): void {
    const regNumber = this.regInput().replace(/\s/g, '').toUpperCase();
    if (!regNumber) return;

    this.isLoading.set(true);
    this.hasSearched.set(false);
    this.result.set(null);
    this.error.set(null);
    this.contactSubmitted.set(false);

    this.http.get<Bilforsikringstilbud>(`${this.apiUrl}/bilforsikring/${regNumber}/tilbud`).subscribe({
      next: (tilbud) => {
        this.result.set(tilbud);
        this.selectedBonus.set(tilbud.bonus);
        this.isLoading.set(false);
        this.hasSearched.set(true);
      },
      error: (err) => {
        this.error.set('Kunne ikke hente pristilbud. Prøv igjen senere.');
        this.isLoading.set(false);
        this.hasSearched.set(true);
        console.error('Error fetching quote:', err);
      }
    });
  }

  onContactSubmit(): void {
    if (!this.phoneNumber() && !this.email()) return;
    if (!this.result()) return;

    this.isSubmittingContact.set(true);

    const request: ContactRequest = {
      regnummer: this.result()!.regnummer.replace(/\s/g, ''),
      telefonnummer: this.phoneNumber(),
      epost: this.email()
    };

    this.http.post(`${this.apiUrl}/kontakt-meg`, request).subscribe({
      next: () => {
        this.isSubmittingContact.set(false);
        this.contactSubmitted.set(true);
      },
      error: (err) => {
        this.isSubmittingContact.set(false);
        console.error('Error submitting contact:', err);
      }
    });
  }

  formatPrice(price: number): string {
    return price.toLocaleString('nb-NO') + ' kr/mnd';
  }

  formatYearlyPrice(monthlyPrice: number): string {
    return (monthlyPrice * 12).toLocaleString('nb-NO') + ' kr/år';
  }

  get isContactFormValid(): boolean {
    return !!(this.phoneNumber() || this.email());
  }
}
