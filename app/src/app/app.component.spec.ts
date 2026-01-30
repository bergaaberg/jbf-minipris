import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AppComponent } from './app.component';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';

describe('AppComponent', () => {
    let component: AppComponent;
    let fixture: ComponentFixture<AppComponent>;

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
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });

    it('should format monthly price correctly', () => {
        const formatted = component.formatPrice(500);
        // Intentionally broken: The actual code adds " kr/mnd" but our test expects just " kr"
        // Candidate needs to either fix the test or the code (likely the test)
        expect(formatted).toBe('500 kr');
    });

    it('should format yearly price correctly', () => {
        const formatted = component.formatYearlyPrice(100);
        expect(formatted).toBe('1 200 kr/år');
    });
});
