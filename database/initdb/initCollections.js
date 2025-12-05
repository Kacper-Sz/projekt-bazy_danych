db = db.getSiblingDB('shop');

db.createCollection('users');
db.createCollection('orders');
db.createCollection('products');

const fs = require('fs');

function readJSON(path) {
    return JSON.parse(fs.readFileSync(path, 'utf8'));
}

const products = readJSON('/docker-entrypoint-initdb.d/Products.json');
const users    = readJSON('/docker-entrypoint-initdb.d/Users.json');
const orders   = readJSON('/docker-entrypoint-initdb.d/Orders.json');

users.forEach(u => {
    u._id = ObjectId(u._id);
    u.createdAt = new Date(u.createdAt);
});

products.forEach(p => {
    p._id = ObjectId(p._id);
});

orders.forEach(o => {
    o._id = ObjectId(o._id);
    o.customerId = ObjectId(o.customerId);
    o.createdAt = new Date(o.createdAt);
    o.items.forEach(i => i.productId = ObjectId(i.productId));
        if (o.totalAmount !== undefined && o.totalAmount !== null) {
        const amountString = 
            typeof o.totalAmount === "object" && o.totalAmount.$numberDecimal
                ? o.totalAmount.$numberDecimal
                : o.totalAmount.toString();

        o.totalAmount = NumberDecimal(amountString);
    }
});

db.products.insertMany(products);
db.users.insertMany(users);
db.orders.insertMany(orders);

print("Database seeded successfully!");