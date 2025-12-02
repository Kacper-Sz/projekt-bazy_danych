
# Readme do projektu z baz danych

## Spis tresci

- [Linki](#Linki)
- [Info ogolne](#Info-ogolne)
- [Struktura repozytorium](#struktura-repozytorium)
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

## Struktura repozytorium

```
naszeRepozytorium/
│
├─ database/ # wszystko co związane z MongoDB
│   ├─ docker-compose.yml
│   ├─ initdb/
│   │   └─ initCollections.js 
│   │   └─ Orders.json
│   │   └─ Orders.json
│   │   └─ Products.json 
│   └─ dump/
│
├─ dll/      
│   └─ (biblioteka z obsługą bazy danych)
│
└─ app/     
    └─ (aplikacja z GUI)
```

## Struktura bazy

baza - sklep (nawet np taka nazwa po prstu sklep)

mialaby 3 kolekcje

1) Products: _id, imageUrl(najlepiej dac url do zdj niz trzymac cale zdjecie w bazie), name, manufacturer, category, price, stock, specs(teoretyczeni to samo co opis tylko wypunktowane by moglo byc), description, createdAt (gdybysmy robili w apce logowanie i tam konta z podzialem na klienta i admina i wtedy admin moze np dodac produkt przez apke albo zmeinic ilosc; innego przydatnego zastosowania nie widze w dacie dodania)

2) Users: _id, firstName, lastName, email, role (klient, admin), password, phoneNumber, createdAt (zeby przychodzil jakis rabat czy cos w dzien urodzin)

3) Orders (po zrobieniu zamowienia w apce leci do bazy i sie wyswietla na np profilu osoby): _id, customerId (w sensie kto zrobil zamowinie), items (same _id produktu by byly wpisane: productId, quantity), status, createdAt, totalAmount, address, paymentMethod

## Przyklady dokumentow

Ponizej przykladowe dane dla trzech kolekcji: `produkty`, `osoby`, `zamowienia`.

### Produkty

```
[
  {
    "_id": { "$oid": "656f1a2b3c4d5e6f7a8b9c01" },
    "imageUrl": "https://cdn.sklep.pl/images/dell-xps13.jpg",
    "name": "Laptop Dell XPS 13",
    "manufacturer": "Dell",
    "category": "Laptopy",
    "price": 5500,
    "stock": 10,
    "specs": {
      "RAM": "16GB",
      "SSD": "512GB",
      "CPU": "Intel i7",
      "Ekran": "13.3 cala"
    },
    "description": "Ultrabook premium z ekranem InfinityEdge i lekką obudową.",
    "createdAt": "2025-11-20T10:00:00Z"
  },
  {
    "_id": { "$oid": "656f1a2b3c4d5e6f7a8b9c02" },
    "imageUrl": "https://cdn.sklep.pl/images/ps5.jpg",
    "name": "Konsola PlayStation 5",
    "manufacturer": "Sony",
    "category": "Konsole",
    "price": 2900,
    "stock": 5,
    "specs": {
      "Dysk": "1TB SSD",
      "GPU": "RDNA 2",
      "RAM": "16GB GDDR6"
    },
    "description": "Nowa generacja konsoli z obsługą ray tracingu i szybkim dyskiem SSD.",
    "createdAt": "2025-11-22T15:30:00Z"
  }
]

```

### Osoby

```
[
  {
    "_id": { "$oid": "656f1a2b3c4d5e6f7a8b9c10" }, // tu jakies losowe _id ktore by sie samo utworzylo przy dodawaniu do bazy
    "firstName": "Jan",
    "lastName": "Kowalski",
    "email": "jan.kowalski@example.com",
    "role": "klient",
    "password": "hashed_password_123",
    "phoneNumber": "+48 600 700 800",
    "createdAt": "2025-10-01T09:00:00Z"
  },
  {
    "_id": { "$oid": "656f1a2b3c4d5e6f7a8b9c11" }, // tu jakies losowe _id ktore by sie samo utworzylo przy dodawaniu do bazy
    "firstName": "Anna",
    "lastName": "Nowak",
    "email": "anna.nowak@example.com",
    "role": "admin",
    "password": "hashed_password_456",
    "phoneNumber": "+48 601 701 801",
    "createdAt": "2025-09-15T12:30:00Z"
  }
]

```

### Zamowienia

```
[
  {
    "_id": { "$oid": "656f1a2b3c4d5e6f7a8b9c20" }, // tu jakies losowe _id ktore by sie samo utworzylo przy dodawaniu do bazy
    "customerId": "jan.kowalski@example.com", // albo to zmienic na klientId i trzymac tam _id z kolekcji osoby
    "items": [
      { "productName": "Laptop Dell XPS 13", "stock": 1 },
      { "productName": "Smartfon Samsung Galaxy S23", "stock": 2 }
    ],
    "status": "w realizacji",
    "createdAt": "2025-11-25T12:00:00Z",
    "totalAmount": 13900,
    "deliveryAddress": {
      "city": "Warszawa",
      "street": "Marszałkowska 10",
      "postalCode": "00-001"
    },
    "paymentMethod": "card"
  }
]

```


