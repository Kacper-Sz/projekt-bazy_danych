db = db.getSiblingDB('shop');

db.createCollection('users');
db.createCollection('orders');
db.createCollection('products');

function readJSON(path) {
    return JSON.parse(cat(path));
}

const products = JSON.parse(require('fs').readFileSync('/docker-entrypoint-initdb.d/Products.json', 'utf8'));
const users    = JSON.parse(require('fs').readFileSync('/docker-entrypoint-initdb.d/Users.json', 'utf8'));
const orders   = JSON.parse(require('fs').readFileSync('/docker-entrypoint-initdb.d/Orders.json', 'utf8'));

db.products.insertMany(products);
db.users.insertMany(users);
db.orders.insertMany(orders);

print("Database seeded successfully!");