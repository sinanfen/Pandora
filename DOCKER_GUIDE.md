# 🐳 Pandora Docker Deployment Guide

## 📋 Prerequisites

- Docker Desktop (Windows/Mac) or Docker Engine (Linux)
- Docker Compose v2.0+
- At least 2GB RAM available for containers

## 🚀 Quick Start

### 1. Clone & Build

```bash
# Clone the repository
git clone https://github.com/sinanfen/Pandora.git
cd Pandora

# Start all services
docker-compose up -d
```

### 2. Wait for Services

```bash
# Check service status
docker-compose ps

# View logs
docker-compose logs -f pandora-api
docker-compose logs -f pandora-db
```

### 3. Access the API

- **API Endpoint:** http://localhost:8080
- **Swagger Documentation:** http://localhost:8080/swagger
- **Health Check:** http://localhost:8080/health

## 🗃️ Services

### 🐘 PostgreSQL Database (`pandora-db`)
- **Port:** 5432
- **Database:** PandoraBox
- **Username:** postgres
- **Password:** postgres
- **Volume:** `pandora_postgres_data`

### 🌐 Pandora API (`pandora-api`)
- **Port:** 8080
- **Environment:** Production
- **Logs Volume:** `pandora_logs`
- **Health Check:** Enabled

### 🔧 pgAdmin (`pandora-pgadmin`) - Optional
- **Port:** 5050
- **Email:** admin@pandora.com
- **Password:** admin123
- **Access:** http://localhost:5050

## 📝 Commands

### Basic Operations

```bash
# Start all services
docker-compose up -d

# Stop all services
docker-compose down

# Restart specific service
docker-compose restart pandora-api

# View logs
docker-compose logs -f [service-name]

# Execute commands in container
docker-compose exec pandora-api bash
docker-compose exec pandora-db psql -U postgres -d PandoraBox
```

### With pgAdmin (Optional)

```bash
# Start with pgAdmin
docker-compose --profile admin up -d

# Stop including pgAdmin
docker-compose --profile admin down
```

### Development & Debugging

```bash
# Build without cache
docker-compose build --no-cache pandora-api

# View container status
docker-compose ps

# Check service health
docker-compose exec pandora-api curl -f http://localhost:8080/health
```

## 🔧 Configuration

### Environment Variables

The Docker Compose file includes all necessary environment variables. Key configurations:

```yaml
# Database Connection
ConnectionStrings__PandoraBoxDatabase: "Host=pandora-db;Port=5432;Database=PandoraBox;Username=postgres;Password=postgres"

# JWT Settings
JwtSettings__SecretKey: "YourSecretKeyHere"
JwtSettings__ExpiresInMinutes: "15"
JwtSettings__RefreshTokenExpiryInDays: "30"

# Email Configuration
Email__SMTP__Host: "smtp.gmail.com"
Email__SMTP__Port: "587"
# ... other email settings
```

### Custom Configuration

To customize settings, edit the `docker-compose.yml` file or create environment-specific override files:

```bash
# Create override for development
docker-compose -f docker-compose.yml -f docker-compose.dev.yml up -d
```

## 🗄️ Database Management

### Automatic Migration

The API automatically runs database migrations on startup.

### Manual Database Operations

```bash
# Connect to PostgreSQL
docker-compose exec pandora-db psql -U postgres -d PandoraBox

# Backup database
docker-compose exec pandora-db pg_dump -U postgres PandoraBox > backup.sql

# Restore database
docker-compose exec -T pandora-db psql -U postgres -d PandoraBox < backup.sql
```

### Using pgAdmin

1. Start with admin profile: `docker-compose --profile admin up -d`
2. Open http://localhost:5050
3. Login with admin@pandora.com / admin123
4. Add server:
   - Host: `pandora-db`
   - Port: `5432`
   - Username: `postgres`
   - Password: `postgres`

## 📊 Monitoring & Logs

### Health Checks

Services include health checks:
- **Database:** PostgreSQL connection test
- **API:** HTTP health endpoint

### Log Management

```bash
# View real-time logs
docker-compose logs -f

# View logs for specific service
docker-compose logs -f pandora-api

# View last 50 log lines
docker-compose logs --tail=50 pandora-api
```

### Log Persistence

Logs are stored in Docker volumes:
- **API Logs:** `pandora_logs` volume
- **Database Logs:** Available via `docker-compose logs pandora-db`

## 🚨 Troubleshooting

### Common Issues

**API not starting:**
```bash
# Check database health
docker-compose exec pandora-db pg_isready -U postgres

# Rebuild API image
docker-compose build --no-cache pandora-api
docker-compose up -d pandora-api
```

**Database connection issues:**
```bash
# Restart database
docker-compose restart pandora-db

# Check database logs
docker-compose logs pandora-db
```

**Port conflicts:**
```bash
# Check if ports are in use
netstat -an | grep :8080
netstat -an | grep :5432

# Change ports in docker-compose.yml if needed
```

### Reset Everything

```bash
# Stop and remove all containers, networks
docker-compose down

# Remove volumes (⚠️ This will delete all data!)
docker-compose down -v

# Remove images
docker-compose down --rmi all

# Start fresh
docker-compose up -d
```

## 🔒 Security Notes

- Change default passwords in production
- Use environment files for sensitive data
- Regular security updates for base images
- Network isolation with custom networks
- Volume encryption for sensitive data

## 📈 Production Deployment

For production deployment:

1. **Update passwords and secrets**
2. **Configure proper email settings**
3. **Set up SSL/TLS termination**
4. **Configure monitoring and alerting**
5. **Set up automated backups**
6. **Use container orchestration (Kubernetes/Docker Swarm)**

## 🤝 Support

For issues and questions:
- Check container logs: `docker-compose logs`
- Review this guide
- Check GitHub issues
- Contact the development team 