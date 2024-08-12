# DigiDock Yönetimi API

DigiDock Yönetimi API'si, dijital ürünlerin yönetimi için çeşitli uç noktalar sağlar.

## 1. Kupon Kodu Yayınla

**Endpoint:** `POST /admin/api/Advertise/PublishCouponCode`

### Parametreler:

| Parametre | Tip     | Açıklama                     |
| :-------- | :------ | :--------------------------- |
| `code`    | `integer` | **Gerekli**. Yayınlanacak kupon kodu. |

### Açıklama:
Verilen kupon kodunu yayınlar.

---

## 2. Kullanıcı Girişi

**Endpoint:** `POST /api/Authorization/Login`

### Parametreler:

| Parametre | Tip      | Açıklama                    |
| :-------- | :------- | :-------------------------- |
| `Email`   | `string` | **Gerekli**. Kullanıcı e-postası. |
| `Password`| `string` | **Gerekli**. Kullanıcı şifresi.  |

### Açıklama:
Kullanıcı giriş işlemi gerçekleştirir.

---

## 3. Kullanıcı Kayıt

**Endpoint:** `POST /api/Authorization/SignIn`

### Parametreler:

| Parametre   | Tip      | Açıklama                     |
| :---------- | :------- | :--------------------------- |
| `FirstName` | `string` | **Gerekli**. Kullanıcının adı.  |
| `LastName`  | `string` | **Gerekli**. Kullanıcının soyadı.  |
| `Email`     | `string` | **Gerekli**. Kullanıcı e-postası. |
| `Password`  | `string` | **Gerekli**. Kullanıcı şifresi. |

### Açıklama:
Yeni bir kullanıcı kaydı oluşturur.

---

## 4. Yeni Admin Oluştur

**Endpoint:** `POST /api/Authorization/CreateNewAdmin`

### Parametreler:

| Parametre   | Tip      | Açıklama                     |
| :---------- | :------- | :--------------------------- |
| `FirstName` | `string` | **Gerekli**. Adminin adı.   |
| `LastName`  | `string` | **Gerekli**. Adminin soyadı. |
| `Email`     | `string` | **Gerekli**. Admin e-postası. |
| `Password`  | `string` | **Gerekli**. Admin şifresi.  |

### Açıklama:
Yeni bir admin kullanıcısı oluşturur.

---

## 5. Sepete Ürün Ekle

**Endpoint:** `POST /api/Cart/AddToCart`

### Parametreler:

| Parametre  | Tip      | Açıklama                     |
| :--------- | :------- | :--------------------------- |
| `ProductId`| `integer`| **Gerekli**. Eklenecek ürün ID'si. |
| `Quantity` | `integer`| **Gerekli**. Eklenilecek miktar.   |

### Açıklama:
Kullanıcının sepetine ürün ekler.

---

## 6. Sepetten Ürün Çıkar

**Endpoint:** `POST /api/Cart/RemoveFromCart`

### Parametreler:

| Parametre  | Tip      | Açıklama                     |
| :--------- | :------- | :--------------------------- |
| `ProductId`| `integer`| **Gerekli**. Çıkarılacak ürün ID'si. |
| `Quantity` | `integer`| **Gerekli**. Çıkarılacak miktar.   |

### Açıklama:
Kullanıcının sepetinden ürün çıkarır.

---

## 7. Sepeti Temizle

**Endpoint:** `POST /api/Cart/ClearCart`

### Parametreler:

| Parametre  | Tip      | Açıklama                     |
| :--------- | :------- | :--------------------------- |
| -          | -        | Sepeti temizler.           |

### Açıklama:
Kullanıcının sepetini tamamen temizler.

---

## 8. Sepeti Getir

**Endpoint:** `GET /api/Cart/GetCart`

### Parametreler:

| Parametre  | Tip      | Açıklama                     |
| :--------- | :------- | :--------------------------- |
| -          | -        | Kullanıcının sepetini getirir. |

### Açıklama:
Kullanıcının mevcut sepetini döndürür.

---

## 9. Ödeme Yap

**Endpoint:** `POST /api/Cart/Checkout`

### Parametreler:

| Parametre       | Tip      | Açıklama                     |
| :-------------- | :------- | :--------------------------- |
| `Address`       | `string` | **Gerekli**. Teslimat adresi. |
| `PaymentMethod` | `string` | **Gerekli**. Ödeme yöntemi.   |
| `CardNumber`    | `string` | **Gerekli**. Kart numarası.    |
| `ExpiryDate`    | `string` | **Gerekli**. Son kullanma tarihi. |
| `CVV`           | `string` | **Gerekli**. CVV kodu.         |
| `CouponCode`    | `string` | İsteğe bağlı. Kupon kodu.     |

### Açıklama:
Kullanıcının sepetinde bulunan ürünler için ödeme işlemini gerçekleştirir.

---

## 10. Sepet Miktarını Güncelle

**Endpoint:** `POST /api/Cart/UpdateCartQuantity`

### Parametreler:

| Parametre  | Tip      | Açıklama                     |
| :--------- | :------- | :--------------------------- |
| `ProductId`| `integer`| **Gerekli**. Güncellenecek ürün ID'si. |
| `Quantity` | `integer`| **Gerekli**. Yeni miktar.     |

### Açıklama:
Kullanıcının sepetindeki ürünün miktarını günceller.

---

## 11. Tüm Kategorileri Getir

**Endpoint:** `GET /api/Category/GetAllCategories`

### Parametreler:

| Parametre  | Tip      | Açıklama                     |
| :--------- | :------- | :--------------------------- |
| -          | -        | Tüm kategorileri getirir.    |

### Açıklama:
Sistemdeki tüm kategorileri listeler.

---

## 12. Kategori Oluştur

**Endpoint:** `POST /api/Category/CreateCategory`

### Parametreler:

| Parametre  | Tip      | Açıklama                     |
| :--------- | :------- | :--------------------------- |
| `Name`     | `string` | **Gerekli**. Kategorinin adı. |
| `Url`      | `string` | **Gerekli**. Kategorinin URL'si. |

### Açıklama:
Yeni bir kategori oluşturur.

---

## 13. Kategoriyi Güncelle

**Endpoint:** `PUT /api/Category/UpdateCategory`

### Parametreler:

| Parametre  | Tip      | Açıklama                     |
| :--------- | :------- | :--------------------------- |
| `Id`       | `integer`| **Gerekli**. Güncellenecek kategori ID'si. |
| `Name`     | `string` | **Gerekli**. Yeni kategori adı.  |
| `Url`      | `string` | **Gerekli**. Yeni kategori URL'si. |

### Açıklama:
Mevcut bir kategoriyi günceller.

---

## 14. Kategoriyi Sil

**Endpoint:** `DELETE /api/Category/DeleteCategory`

### Parametreler:

| Parametre  | Tip      | Açıklama                     |
| :--------- | :------- | :--------------------------- |
| `id`       | `integer`| **Gerekli**. Silinecek kategori ID'si. |

### Açıklama:
Belirtilen kategori ID'sine sahip kategoriyi siler.

---

## 15. Kuponları Getir

**Endpoint:** `GET /admin/api/Coupon/GetAllCoupons`

### Parametreler:

| Parametre  | Tip      | Açıklama                     |
| :--------- | :------- | :--------------------------- |
| -          | -        | Tüm kuponları getirir.      |

### Açıklama:
Sistemdeki tüm kuponları listeler.

---

## 16. Aktif Kuponları Getir

**Endpoint:** `GET /admin/api/Coupon/GetActiveCoupons`

### Parametreler:

| Parametre  | Tip      | Açıklama                     |
| :--------- | :------- | :--------------------------- |
| -          | -        | Aktif kuponları getirir.    |

### Açıklama:
Aktif olan kuponları listeler.
## 17. Kupon Oluştur

**Endpoint:** `POST /admin/api/Coupon/CreateCoupon`

### Parametreler:

| Parametre   | Tip      | Açıklama                     |
| :---------- | :------- | :--------------------------- |
| `Code`      | `string` | **Gerekli**. Oluşturulacak kupon kodu. |
| `Discount`  | `decimal`| **Gerekli**. Kupon indirimi.          |
| `ExpiryDate`| `DateTime`| **Gerekli**. Kuponun son kullanma tarihi. |

### Açıklama:
Yeni bir kupon oluşturur.

---

## 18. Kupon Güncelle

**Endpoint:** `PUT /admin/api/Coupon/UpdateCoupon`

### Parametreler:

| Parametre   | Tip      | Açıklama                     |
| :---------- | :------- | :--------------------------- |
| `Id`        | `integer`| **Gerekli**. Güncellenecek kupon ID'si. |
| `Code`      | `string` | **Gerekli**. Yeni kupon kodu.           |
| `Discount`  | `decimal`| **Gerekli**. Yeni kupon indirimi.       |
| `ExpiryDate`| `DateTime`| **Gerekli**. Yeni son kullanma tarihi.  |

### Açıklama:
Belirtilen kuponu günceller.

---

## 19. Kupon Sil

**Endpoint:** `DELETE /admin/api/Coupon/DeleteCoupon`

### Parametreler:

| Parametre   | Tip      | Açıklama                     |
| :---------- | :------- | :--------------------------- |
| `Id`        | `integer`| **Gerekli**. Silinecek kupon ID'si. |

### Açıklama:
Belirtilen kuponu siler.

---

## 20. Ürün Oluştur

**Endpoint:** `POST /admin/api/Product/CreateProduct`

### Parametreler:

| Parametre      | Tip      | Açıklama                     |
| :------------- | :------- | :--------------------------- |
| `Name`         | `string` | **Gerekli**. Ürün adı.      |
| `Description`  | `string` | **Gerekli**. Ürün açıklaması. |
| `Price`        | `decimal`| **Gerekli**. Ürün fiyatı.   |
| `CategoryId`   | `integer`| **Gerekli**. Ürün kategorisi ID'si. |
| `StockQuantity`| `integer`| **Gerekli**. Ürün stoğu.    |

### Açıklama:
Yeni bir ürün oluşturur.

---

## 21. Ürün Güncelle

**Endpoint:** `PUT /admin/api/Product/UpdateProduct`

### Parametreler:

| Parametre      | Tip      | Açıklama                     |
| :------------- | :------- | :--------------------------- |
| `Id`           | `integer`| **Gerekli**. Güncellenecek ürün ID'si. |
| `Name`         | `string` | **Gerekli**. Yeni ürün adı. |
| `Description`  | `string` | **Gerekli**. Yeni ürün açıklaması. |
| `Price`        | `decimal`| **Gerekli**. Yeni ürün fiyatı. |
| `CategoryId`   | `integer`| **Gerekli**. Yeni ürün kategorisi ID'si. |
| `StockQuantity`| `integer`| **Gerekli**. Yeni ürün stoğu. |

### Açıklama:
Belirtilen ürünü günceller.

---

## 22. Ürün Sil

**Endpoint:** `DELETE /admin/api/Product/DeleteProduct`

### Parametreler:

| Parametre      | Tip      | Açıklama                     |
| :------------- | :------- | :--------------------------- |
| `Id`           | `integer`| **Gerekli**. Silinecek ürün ID'si. |

### Açıklama:
Belirtilen ürünü siler.

---

## 23. Ürünleri Getir

**Endpoint:** `GET /api/Product/GetAllProducts`

### Parametreler:

| Parametre      | Tip      | Açıklama                     |
| :------------- | :------- | :--------------------------- |
| -              | -        | Tüm ürünleri getirir.       |

### Açıklama:
Sistemdeki tüm ürünleri listeler.

---
