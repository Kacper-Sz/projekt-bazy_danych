
# Readme do projektu z baz danych

## Spis tresci

- [Linki](#Linki)
- [Info ogolne](#Info-ogolne)
- [Struktura bazy](#struktura-bazy)
- [Przyklady dokumentow](#przyklady-dokumentow)
  - [Produkty](#produkty)
  - [Osoby](#osoby)
  - [Zamowienia](#zamowienia)

---

## To co udalo mi sie poszukac z gotowcow - nic w sumie z tego nie uzylem
## Linki

- https://github.com/Tony641/MongoDB-Ecommerce-Database?tab=readme-ov-file#
- https://github.com/ozlerhakan/mongodb-json-files
- https://github.com/neelabalan/mongodb-sample-dataset
- https://github.com/ayubamini/Online-Market
- https://www.mongodb.com/docs/atlas/sample-data/sample-supplies/

## Info ogolne

Myslalem zeby zobic sklep elektorniczny/z czesciami do kompa i wszytkie przykaldy i pola robilem pod taka tematyke.

## Struktura bazy

baza - sklep (nawet np taka nazwa po prstu sklep)

mialaby 3 kolekcje

1) produkty z polami: _id, zdjecie(najlepiej dac imageUrl niz trzymac cale zdjecie w bazie), nazwa, producent, kategoria, cena, ilosc, specyfikacja(teoretyczeni to samo co opis tylko wypunktowane by moglo byc), opis, data dodania do bazy (gdybysmy robili w apce logowanie i tam konta z podzialem na klienta i admina i wtedy admin moze np dodac produkt przez apke albo zmeinic ilosc; innego przydatnego zastosowania nie widze w dacie dodania)

2) osoby: _id, imie, nazwisko, email, typ konta (klient, admin), haslo, telefon, data zalozenia konta (zeby przychodzil jakis rabat czy cos w dzien urodzin)

3) zamowienia (po zrobieniu zamowienia w apce leci do bazy i sie wyswietla na np profilu osoby): _id, emailklienta/ id klienta (w sensie kto zrobil zamowinie), produkty (same _id produktu by byly wpisane), status, data zamowienia, wartosc zamowienia, adres dostway, metoda platnosci

## Przyklady dokumentow

Ponizej przykladowe dane dla trzech kolekcji: `produkty`, `osoby`, `zamowienia`.

### Produkty

```
[
  {
    "_id": { "$oid": "656f1a2b3c4d5e6f7a8b9c01" },
    "imageUrl": "https://cdn.sklep.pl/images/dell-xps13.jpg",
    "nazwa": "Laptop Dell XPS 13",
    "producent": "Dell",
    "kategoria": "Laptopy",
    "cena": 5500,
    "ilosc": 10,
    "specyfikacja": {
      "RAM": "16GB",
      "SSD": "512GB",
      "CPU": "Intel i7",
      "Ekran": "13.3 cala"
    },
    "opis": "Ultrabook premium z ekranem InfinityEdge i lekką obudową.",
    "dataDodania": "2025-11-20T10:00:00Z"
  },
  {
    "_id": { "$oid": "656f1a2b3c4d5e6f7a8b9c02" },
    "imageUrl": "https://cdn.sklep.pl/images/ps5.jpg",
    "nazwa": "Konsola PlayStation 5",
    "producent": "Sony",
    "kategoria": "Konsole",
    "cena": 2900,
    "ilosc": 5,
    "specyfikacja": {
      "Dysk": "1TB SSD",
      "GPU": "RDNA 2",
      "RAM": "16GB GDDR6"
    },
    "opis": "Nowa generacja konsoli z obsługą ray tracingu i szybkim dyskiem SSD.",
    "dataDodania": "2025-11-22T15:30:00Z"
  }
]

```

### Osoby

```
[
  {
    "_id": { "$oid": "656f1a2b3c4d5e6f7a8b9c10" }, // tu jakies losowe _id ktore by sie samo utworzylo przy dodawaniu do bazy
    "imie": "Jan",
    "nazwisko": "Kowalski",
    "email": "jan.kowalski@example.com",
    "typKonta": "klient",
    "haslo": "hashed_password_123",
    "telefon": "+48 600 700 800",
    "dataZalozenia": "2025-10-01T09:00:00Z"
  },
  {
    "_id": { "$oid": "656f1a2b3c4d5e6f7a8b9c11" }, // tu jakies losowe _id ktore by sie samo utworzylo przy dodawaniu do bazy
    "imie": "Anna",
    "nazwisko": "Nowak",
    "email": "anna.nowak@example.com",
    "typKonta": "admin",
    "haslo": "hashed_password_456",
    "telefon": "+48 601 701 801",
    "dataZalozenia": "2025-09-15T12:30:00Z"
  }
]

```

### Zamowienia

```
[
  {
    "_id": { "$oid": "656f1a2b3c4d5e6f7a8b9c20" }, // tu jakies losowe _id ktore by sie samo utworzylo przy dodawaniu do bazy
    "klientEmail": "jan.kowalski@example.com", // albo to zmienic na klientId i trzymac tam _id z kolekcji osoby
    "produkty": [
      { "produktNazwa": "Laptop Dell XPS 13", "ilosc": 1 },
      { "produktNazwa": "Smartfon Samsung Galaxy S23", "ilosc": 2 }
    ],
    "status": "w realizacji",
    "dataZamowienia": "2025-11-25T12:00:00Z",
    "wartoscZamowienia": 13900,
    "adresDostawy": {
      "miasto": "Warszawa",
      "ulica": "Marszałkowska 10",
      "kod": "00-001"
    },
    "metodaPlatnosci": "karta"
  }
]

```

