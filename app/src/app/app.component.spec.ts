import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AppComponent } from './app.component';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { beforeEach, describe, expect, it } from "vitest";

describe('AppComponent', () => {
    let component: AppComponent;
    let fixture: ComponentFixture<AppComponent>;
    let httpMock: HttpTestingController;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            imports: [AppComponent],
            providers: [
                provideHttpClient(),
                provideHttpClientTesting()
            ]
        }).compileComponents();

        fixture = TestBed.createComponent(AppComponent);
        component = fixture.componentInstance;
        httpMock = TestBed.inject(HttpTestingController);
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });

    it('should format monthly price correctly', () => {
        const formatted = component.formatPrice(500);
        expect(formatted).toBe('500 kr/mnd');
    });

    it('should format yearly price correctly', () => {
        const formatted = component.formatYearlyPrice(100);
        expect(formatted.replace(/\s/g, ' ')).toBe('1 200 kr/år');
    });

    it('should calculate yearly from monthly price', () => {
        const yearly = component.formatYearlyPrice(50);
        expect(yearly).toBe('600 kr/år');
    });

    it('should have correct number of bonus options', () => {
        expect(component.bonusOptions.length).toBe(9);
    });

    it('should include max bonus of 75%', () => {
        expect(component.bonusOptions).toContain(75);
    });

    it('should validate contact form requires phone or email', () => {
        component.phoneNumber.set('');
        component.email.set('');
        expect(component.isContactFormValid).toBe(false);

        component.phoneNumber.set('12345678');
        component.email.set('');
        expect(component.isContactFormValid).toBe(true);

        component.phoneNumber.set('');
        component.email.set('test@example.com');
        expect(component.isContactFormValid).toBe(true);

        component.phoneNumber.set('12345678');
        component.email.set('test@example.com');
        expect(component.isContactFormValid).toBe(true);
    });

    it('should normalize registration number and call correct API endpoint', () => {
        component.regInput.set('ab 12345');
        component.onSubmit();

        const req = httpMock.expectOne('/api/bilforsikring/AB12345/tilbud');
        expect(req.request.method).toBe('GET');

        req.flush({
            regnummer: 'AB 12345',
            merke: 'Toyota',
            modell: 'Corolla',
            arsmodell: 2020,
            forsikringspris: 500,
            dekning: 'Kasko',
            bonus: 60,
            egenandel: 4000,
            dekningsalternativer: []
        });

        expect(component.result()?.merke).toBe('Toyota');
    });

    it('should set loading state to false before API call completes', () => {
        component.regInput.set('CD67890');
        component.onSubmit();

        expect(component.isLoading()).toBe(true);

        const req = httpMock.expectOne('/api/bilforsikring/CD67890/tilbud');
        req.flush({
            regnummer: 'CD 67890',
            merke: 'Volvo',
            modell: 'XC60',
            arsmodell: 2022,
            forsikringspris: 750,
            dekning: 'Delkasko',
            bonus: 50,
            egenandel: 6000,
            dekningsalternativer: []
        });
    });
});
