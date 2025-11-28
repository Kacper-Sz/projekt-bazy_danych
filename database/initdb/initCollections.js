db = db.getSiblingDB('shop');

db.createCollection('users');
db.createCollection('orders');
db.createCollection('products');