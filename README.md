# WebShop
Webshop Mircoservices for Airfi Networks


API Endpoint Items 

GET - http://localhost:49757/api/items | This will get the current stock of item

API Endpoint Cart 

GET - http://localhost:50173/api/cart/{cartId} | This will get current items in your cart 

POST - http://localhost:50173/api/cart | This will allow you to intially create your cart and provide an id

PUT - http://localhost:50173/api/cart/add/{cartId} | This will allow you to add new items to the current cart 

PUT - http://localhost:50173/api/cart/remove/{cartId} | This will allow you to remove items from the current cart 

API Endpoint Orders

POST - http://localhost:50179/api/orders/register | This will allow you to register and the id returned is required for checkout

PUT - http://localhost:50179/api/orders/checkout/{cartId}/{userId} | This allows you to checkout the cart you made 

GET - http://localhost:50179/api/orders/{userId} | This allows you to get allow orders made by the user


