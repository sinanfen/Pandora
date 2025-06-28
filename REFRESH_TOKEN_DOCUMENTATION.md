# 🔐 Pandora API - Refresh Token Sistemi Dokümantasyonu

## 📖 İçindekiler

1. [Refresh Token Nedir?](#refresh-token-nedir)
2. [Neden Kullanılır?](#neden-kullanılır)
3. [Nasıl Çalışır?](#nasıl-çalışır)
4. [API Endpoint'leri](#api-endpointleri)
5. [Kullanım Örnekleri](#kullanım-örnekleri)
6. [Mobil Uygulama Entegrasyonu](#mobil-uygulama-entegrasyonu)
7. [Güvenlik Özellikleri](#güvenlik-özellikleri)
8. [Konfigürasyon](#konfigürasyon)
9. [Database Schema](#database-schema)
10. [Best Practices](#best-practices)
11. [Troubleshooting](#troubleshooting)

---

## 🎯 Refresh Token Nedir?

**Refresh Token**, JWT tabanlı kimlik doğrulama sistemlerinde kullanılan **uzun süreli** bir güvenlik token'ıdır. İki tür token ile çalışır:

### 🎫 Token Türleri

| Token Türü | Süre | Amaç | Saklama Yeri |
|------------|------|------|--------------|
| **Access Token (JWT)** | 15 dakika | API istekleri için | Memory (RAM) |
| **Refresh Token** | 30 gün | Yeni access token almak için | Güvenli Storage |

---

## 🤔 Neden Kullanılır?

### ❌ **Refresh Token OLMADAN:**
```
Kullanıcı → Login → 15dk sonra token süresi dolar → Tekrar login iste!
```
- Kullanıcı her 15 dakikada bir şifresini girmek zorunda
- Kötü kullanıcı deneyimi
- Mobil uygulamalarda kullanılamaz duruma geliyor

### ✅ **Refresh Token İLE:**
```
Kullanıcı → Login → 30 gün boyunca otomatik token yenileme → Sorunsuz kullanım!
```
- Kullanıcı bir kez login olur
- Sistem arka planda otomatik olarak token'ları yeniler
- 30 gün boyunca kesintisiz kullanım
- Mükemmel mobil deneyim

---

## ⚙️ Nasıl Çalışır?

### 🔄 **Token Döngüsü:**

```
👤 Kullanıcı Login
    ↓
🎫 Access + Refresh Token Al
    ↓
📱 API İstekleri Yap
    ↓
🕐 Access Token Süresi Doldu mu?
    ↓ (Evet)
🔄 Refresh Token Kullan
    ↓
🆕 Yeni Access Token Al
    ↓
🔄 Refresh Token'ı Yenile
    ↓
📱 API İsteklerine Devam Et
```

### 📝 **Adım Adım Süreç:**

1. **Login:** Kullanıcı username/password ile giriş yapar
2. **Token Üretimi:** Sistem Access (15dk) + Refresh (30 gün) token verir
3. **API Kullanımı:** Mobil app Access Token ile API'yi kullanır
4. **Token Süresi Dolunca:** API 401 Unauthorized döner
5. **Otomatik Yenileme:** App, Refresh Token ile yeni Access Token alır
6. **Token Rotation:** Eski Refresh Token silinir, yenisi verilir
7. **Devam:** Kullanıcı kesintisiz kullanmaya devam eder

---

## 🌐 API Endpoint'leri

### 1. 🚪 **Kullanıcı Girişi**
```http
POST /api/auth/login
Content-Type: application/json

{
  "usernameOrEmail": "kullanici@email.com",
  "password": "sifre123"
}
```

**Başarılı Response:**
```json
{
  "resultStatus": "Success",
  "message": "Login successful",
  "data": {
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "w5PJ7x3kR8mN2vC9bF6tY1qE4zB8pN7mK3sC...",
    "accessTokenExpiresAt": 1701234567,
    "refreshTokenExpiresAt": 1703826567,
    "tokenType": "Bearer"
  }
}
```

### 2. 🔄 **Token Yenileme**
```http
POST /api/auth/refresh
Content-Type: application/json

{
  "refreshToken": "w5PJ7x3kR8mN2vC9bF6tY1qE4zB8pN7mK3sC..."
}
```

**Response:**
```json
{
  "resultStatus": "Success",
  "message": "Token refreshed successfully",
  "data": {
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "X9mK3sC2vC9bF6tY1qE4zB8pN7wP5J7x3k...",
    "accessTokenExpiresAt": 1701235467,
    "refreshTokenExpiresAt": 1703827467,
    "tokenType": "Bearer"
  }
}
```

### 3. 🚪 **Çıkış (Logout)**
```http
POST /api/auth/logout
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### 4. 📱 **Aktif Oturumları Görme**
```http
GET /api/auth/sessions?currentRefreshToken=w5PJ7x3kR8mN2vC9bF6tY1qE...
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### 5. 🔒 **Diğer Cihazlardan Çıkış**
```http
POST /api/auth/logout-others
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

"w5PJ7x3kR8mN2vC9bF6tY1qE4zB8pN7mK3sC..."
```

### 6. ❌ **Token İptal Etme**
```http
POST /api/auth/revoke
Content-Type: application/json

{
  "refreshToken": "w5PJ7x3kR8mN2vC9bF6tY1qE4zB8pN7mK3sC...",
  "reason": "User requested token revocation"
}
```

---

## 💻 Kullanım Örnekleri

### 📱 **React Native / Flutter Örneği**

```typescript
class AuthManager {
  private accessToken: string | null = null;
  private refreshToken: string | null = null;

  // 1. İlk Login
  async login(email: string, password: string) {
    const response = await fetch('/api/auth/login', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        usernameOrEmail: email,
        password: password
      })
    });

    const result = await response.json();
    
    if (result.resultStatus === 'Success') {
      this.accessToken = result.data.accessToken;
      this.refreshToken = result.data.refreshToken;
      
      // Güvenli şekilde sakla
      await this.secureStorage.setItem('refreshToken', this.refreshToken);
      
      return true;
    }
    return false;
  }

  // 2. Otomatik Token Yenileme ile API İsteği
  async makeApiRequest(url: string, options: RequestInit = {}) {
    // İlk deneme
    let response = await this.makeRequest(url, options);
    
    // Token süresi dolmuşsa
    if (response.status === 401) {
      const refreshed = await this.refreshTokens();
      
      if (refreshed) {
        // Yeni token ile tekrar dene
        response = await this.makeRequest(url, options);
      } else {
        // Refresh başarısızsa login sayfasına yönlendir
        this.redirectToLogin();
        throw new Error('Authentication failed');
      }
    }
    
    return response;
  }

  // 3. Token Yenileme
  private async refreshTokens(): Promise<boolean> {
    if (!this.refreshToken) return false;

    try {
      const response = await fetch('/api/auth/refresh', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          refreshToken: this.refreshToken
        })
      });

      const result = await response.json();
      
      if (result.resultStatus === 'Success') {
        this.accessToken = result.data.accessToken;
        this.refreshToken = result.data.refreshToken;
        
        // Yeni refresh token'ı güvenli şekilde sakla
        await this.secureStorage.setItem('refreshToken', this.refreshToken);
        
        return true;
      }
    } catch (error) {
      console.error('Token refresh failed:', error);
    }
    
    return false;
  }

  // 4. Authorization Header ile İstek
  private async makeRequest(url: string, options: RequestInit) {
    return fetch(url, {
      ...options,
      headers: {
        ...options.headers,
        'Authorization': `Bearer ${this.accessToken}`,
        'Content-Type': 'application/json'
      }
    });
  }

  // 5. Çıkış
  async logout() {
    if (this.accessToken) {
      await fetch('/api/auth/logout', {
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${this.accessToken}`
        }
      });
    }
    
    // Local storage'ı temizle
    this.accessToken = null;
    this.refreshToken = null;
    await this.secureStorage.removeItem('refreshToken');
  }
}
```

---

## 📱 Mobil Uygulama Entegrasyonu

### 🔐 **Güvenli Saklama**

#### iOS (Swift)
```swift
// Keychain'de Refresh Token Saklama
import Security

class KeychainHelper {
    static func save(key: String, data: Data) -> Bool {
        let query = [
            kSecClass: kSecClassGenericPassword,
            kSecAttrAccount: key,
            kSecValueData: data
        ] as CFDictionary
        
        SecItemDelete(query)
        return SecItemAdd(query, nil) == errSecSuccess
    }
    
    static func load(key: String) -> Data? {
        let query = [
            kSecClass: kSecClassGenericPassword,
            kSecAttrAccount: key,
            kSecReturnData: true
        ] as CFDictionary
        
        var result: AnyObject?
        SecItemCopyMatching(query, &result)
        return result as? Data
    }
}
```

#### Android (Kotlin)
```kotlin
// EncryptedSharedPreferences ile Saklama
import androidx.security.crypto.EncryptedSharedPreferences
import androidx.security.crypto.MasterKey

class SecureStorage(context: Context) {
    private val masterKey = MasterKey.Builder(context)
        .setKeyScheme(MasterKey.KeyScheme.AES256_GCM)
        .build()
    
    private val sharedPreferences = EncryptedSharedPreferences.create(
        context,
        "secure_prefs",
        masterKey,
        EncryptedSharedPreferences.PrefKeyEncryptionScheme.AES256_SIV,
        EncryptedSharedPreferences.PrefValueEncryptionScheme.AES256_GCM
    )
    
    fun saveRefreshToken(token: String) {
        sharedPreferences.edit()
            .putString("refresh_token", token)
            .apply()
    }
    
    fun getRefreshToken(): String? {
        return sharedPreferences.getString("refresh_token", null)
    }
}
```

---

## 🔒 Güvenlik Özellikleri

### 🛡️ **Implement Edilen Güvenlik Önlemleri:**

1. **🔄 Token Rotation**
   - Her refresh'te hem access hem refresh token yenilenir
   - Eski token'lar otomatik iptal edilir

2. **📍 IP ve Device Tracking**
   - Her token hangi IP'den oluşturulduğunu kaydeder
   - User-Agent bilgisi ile cihaz takibi

3. **⏰ Automatic Expiry**
   - Access Token: 15 dakika
   - Refresh Token: 30 gün
   - Background service ile otomatik temizlik

4. **🚫 Token Revocation**
   - Manual token iptal etme
   - Cascade revocation (ilişkili token'ları da iptal)
   - Logout all devices özelliği

5. **🔗 Token Chaining**
   - Hangi token'ın hangi token'dan üretildiği kaydedilir
   - Security breach durumunda tüm chain iptal edilebilir

6. **📊 Session Management**
   - Aktif oturumları görme
   - Belirli cihazları çıkış yaptırma
   - Şüpheli aktivite takibi

---

## ⚙️ Konfigürasyon

### 📄 **appsettings.json**
```json
{
  "JwtSettings": {
    "SecretKey": "MySecretKey12345MySecretKey12345MySecretKey12345MySecretKey12345",
    "Issuer": "Pandora.API",
    "Audience": "Pandora.Users",
    "ExpiresInMinutes": 15,
    "RefreshTokenExpiryInDays": 30
  }
}
```

### 🔧 **Özelleştirilebilir Ayarlar:**

| Ayar | Açıklama | Önerilen Değer |
|------|----------|----------------|
| `ExpiresInMinutes` | Access token süresi | 15-60 dakika |
| `RefreshTokenExpiryInDays` | Refresh token süresi | 30-90 gün |
| `SecretKey` | JWT imzalama anahtarı | Minimum 256 bit |

---

## 🗄️ Database Schema

### 📋 **RefreshTokens Tablosu**
```sql
CREATE TABLE RefreshTokens (
    Id                  UUID PRIMARY KEY,
    UserId              UUID NOT NULL REFERENCES Users(Id) ON DELETE CASCADE,
    Token               VARCHAR(500) UNIQUE NOT NULL,
    ExpiresAt           TIMESTAMP NOT NULL,
    IsUsed              BOOLEAN NOT NULL DEFAULT FALSE,
    IsRevoked           BOOLEAN NOT NULL DEFAULT FALSE,
    IpAddress           VARCHAR(45),
    UserAgent           VARCHAR(500),
    ReplacedByTokenId   UUID,
    RevocationReason    VARCHAR(200),
    CreatedDate         TIMESTAMP NOT NULL,
    UpdatedDate         TIMESTAMP
);
```

### 📊 **Örnek Veriler**
```sql
-- Aktif kullanıcı oturumu
INSERT INTO RefreshTokens VALUES (
    'a1b2c3d4-e5f6-7890-abcd-ef1234567890',  -- Id
    '12345678-90ab-cdef-1234-567890abcdef',  -- UserId
    'w5PJ7x3kR8mN2vC9bF6tY1qE4zB8pN7mK3sC2hG9vF4xZ8bM1nQ5pL7dR6tY3wE', -- Token
    '2023-12-20 15:30:00',                   -- ExpiresAt
    FALSE,                                   -- IsUsed
    FALSE,                                   -- IsRevoked
    '192.168.1.100',                        -- IpAddress
    'Mozilla/5.0 (iPhone; CPU iPhone OS 17_0 like Mac OS X)', -- UserAgent
    NULL,                                    -- ReplacedByTokenId
    NULL,                                    -- RevocationReason
    '2023-11-20 15:30:00',                  -- CreatedDate
    NULL                                     -- UpdatedDate
);
```

---

## 💡 Best Practices

### 📱 **Mobil Uygulama**

#### ✅ **Yapılması Gerekenler:**
- **Access Token'ı sadece memory'de** sakla (RAM)
- **Refresh Token'ı encrypted storage'da** sakla (Keychain/Keystore)
- **Otomatik refresh mantığı** kur
- **Network hatalarını handle** et
- **Token expiry'yi kontrol** et
- **Logout'ta tüm token'ları temizle**

#### ❌ **Yapılmaması Gerekenler:**
- Token'ları **plain text** olarak saklama
- **HTTP** üzerinden token gönderme
- Token'ları **log'lara** yazdırma
- **Browser cache'de** token bırakma

### 🖥️ **Backend**

#### ✅ **Güvenlik Önerileri:**
```csharp
// 1. Rate Limiting
[EnableRateLimiting("RefreshTokenPolicy")]
public async Task<IActionResult> RefreshToken()

// 2. HTTPS Only
services.AddHsts(options => {
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(365);
});

// 3. Secure Headers
app.UseSecurityHeaders(policies => {
    policies.AddDefaultSecurityHeaders();
    policies.AddStrictTransportSecurity();
});
```

### 📊 **Monitoring ve Logging**

```csharp
// Refresh token kullanımını logla
_logger.LogInformation("Refresh token used", new {
    UserId = user.Id,
    IpAddress = ipAddress,
    UserAgent = userAgent,
    TokenId = refreshToken.Id
});

// Şüpheli aktiviteleri logla
_logger.LogWarning("Multiple refresh attempts", new {
    UserId = user.Id,
    IpAddress = ipAddress,
    AttemptCount = attemptCount
});
```

---

## 🚨 Troubleshooting

### ❓ **Sık Karşılaşılan Sorunlar**

#### 1. **"Invalid refresh token" Hatası**
```json
{
  "resultStatus": "Error",
  "message": "Invalid refresh token"
}
```
**Çözüm:**
- Token'ın doğru formatta gönderildiğini kontrol edin
- Token'ın süresi dolmamış olduğunu kontrol edin
- Kullanıcıyı tekrar login olmaya yönlendirin

#### 2. **"Token already used" Hatası**
**Sebep:** Aynı refresh token birden fazla kez kullanılmış
**Çözüm:** Bu güvenlik önlemidir, kullanıcıyı login sayfasına yönlendirin

#### 3. **Migration Hatası**
```bash
# Migration çalıştırma
cd Pandora.Infrastructure
dotnet ef database update
```

#### 4. **Performance Sorunları**
**Çözüm:**
- Database index'lerini kontrol edin
- Expired token cleanup job'ının çalıştığını doğrulayın
- Connection pool ayarlarını optimize edin

### 🔍 **Debug için SQL Sorguları**

```sql
-- Kullanıcının aktif token'larını listele
SELECT * FROM RefreshTokens 
WHERE UserId = 'user-id-here' 
  AND IsRevoked = FALSE 
  AND IsUsed = FALSE 
  AND ExpiresAt > NOW();

-- Süresi dolmuş token'ları bul
SELECT COUNT(*) FROM RefreshTokens 
WHERE ExpiresAt <= NOW();

-- En çok kullanılan IP adreslerini listele
SELECT IpAddress, COUNT(*) as TokenCount 
FROM RefreshTokens 
GROUP BY IpAddress 
ORDER BY TokenCount DESC 
LIMIT 10;
```

---

## 📞 Support

Sorularınız için:
- **GitHub Issues**: Projeye issue açın
- **Documentation**: Bu dokümantasyonu takip edin
- **Code Review**: Pull request gönderin

---

## 🎉 Özet

Bu refresh token sistemi sayesinde:

✅ **Kullanıcılarınız** 30 gün boyunca kesintisiz mobil deneyim yaşayacak  
✅ **Güvenlik** en üst seviyede korunacak  
✅ **Multi-device** desteği tam olacak  
✅ **Session management** ile kontrol sizde olacak  
✅ **Scalable** ve **maintainable** bir yapınız olacak  

**Sistem hazır! Migration'ı çalıştırın ve kullanmaya başlayın! 🚀** 