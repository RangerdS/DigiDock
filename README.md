# DigiDock
Patika &amp; Papara Final Project 

Endpoints Overview
1. Advertise
POST /admin/api/Advertise/PublishCouponCode: Allows admins to publish a coupon code.
2. Authorization
POST /api/Authorization/Login: Endpoint for user login.
POST /api/Authorization/SignIn: Endpoint for user registration.
POST /api/Authorization/CreateNewAdmin: Allows the creation of a new admin user.
3. Cart
POST /api/Cart/AddToCart: Adds a product to the cart.
POST /api/Cart/RemoveFromCart: Removes a product from the cart.
POST /api/Cart/ClearCart: Clears the cart.
GET /api/Cart/GetCart: Retrieves the contents of the cart.
POST /api/Cart/Checkout: Proceeds to checkout with the current cart.
PUT /api/Cart/UpdateCartQuantity: Updates the quantity of a specific item in the cart.
4. Category
GET /api/Category/GetAllCategories: Retrieves a list of all categories.
POST /api/Category/CreateCategory: Creates a new category.
PUT /api/Category/UpdateCategory: Updates an existing category.
DELETE /api/Category/DeleteCategory: Deletes a category.
POST /api/Category/AddProductToCategory: Adds a product to a specific category.
5. Coupon
GET /admin/api/Coupon/GetAllCoupons: Retrieves a list of all coupons.
GET /admin/api/Coupon/GetActiveCoupons: Retrieves a list of all active coupons.
POST /admin/api/Coupon/CreateCoupon: Creates a new coupon.
PUT /admin/api/Coupon/UpdateCoupon: Updates an existing coupon.
DELETE /admin/api/Coupon/DeleteCoupon: Deletes a coupon.
6. Order
GET /api/Order/active: Retrieves active orders.
GET /api/Order/history: Retrieves order history.
7. Product
GET /api/Product/GetAllProducts: Retrieves a list of all products.
POST /api/Product/CreateProduct: Creates a new product.
PUT /api/Product/UpdateProduct: Updates an existing product.
DELETE /api/Product/DeleteProduct: Deletes a product.
8. User
PUT /api/User/UpdateUserPassword: Updates the user's password.
DELETE /api/User/DeleteUser: Deletes a user account.
GET /api/User/GetUserPoint: Retrieves the user's points.
