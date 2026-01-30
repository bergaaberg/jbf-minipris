# JBF Minipris

Applikasjon for pristilbud på bilforsikring med Angular frontend og .NET minimal API backend.

## Prosjektstruktur

```
jbf-minipris/
├── app/                    # Angular frontend
│   ├── src/
│   │   ├── app/           # Angular-komponenter
│   │   ├── assets/        # Ikoner, bilder, fonter
│   │   └── styles.scss    # Globale stiler
│   ├── angular.json
│   ├── package.json
│   └── proxy.conf.json    # API-proxy for utvikling
│
└── backend/
    ├── Minipris/           # .NET minimal API backend
    │   ├── Features/
    │   │   ├── Cars/          # Bilforsikringstilbud
    │   │   ├── Contact/       # Håndtering av kontaktskjema
    │   ├── Properties/
    │   │   └── launchSettings.json
    │   ├── Program.cs
    │   └── Minipris.csproj
    ├── Tests/
    └── Minipris.slnx
```

## Kjør applikasjonen

### Backend (.NET API)

```bash
cd backend/Minipris
dotnet run
```

API-et vil starte på `http://localhost:5000`. Swagger UI er tilgjengelig på `http://localhost:5000/swagger`.

### Frontend (Angular)

```bash
cd app
npm install
npm start
```

Angular-appen vil starte på `http://localhost:4200` og videresende API-forespørsler til backenden.

## API-endepunkter

- `GET /api/cars/{regNumber}/quote` - Hent forsikringstilbud for en bil
- `POST /api/contact` - Send kontaktskjema

## Mock-data

Følgende registreringsnumre har forhåndsdefinerte data:
- AB12345 - Toyota RAV4 (2020)
- CD67890 - Volkswagen Golf (2019)
- EF11111 - Tesla Model 3 (2022)
- GH22222 - Volvo XC60 (2021)
- IJ33333 - BMW 3-serie (2018)

Andre registreringsnumre vil returnere tilfeldig genererte bildata.
