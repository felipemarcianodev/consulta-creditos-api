# Guia de containerização

## Visão geral

Este projeto utiliza Docker e Docker Compose para containerização da aplicação e seus serviços dependentes.

## Arquitetura de containers

### Serviços

1. **API (consulta-creditos-api)**
   - Imagem: .NET 9.0 runtime
   - Porta: 8080
   - Build multi-stage para otimização

2. **PostgreSQL**
   - Imagem: postgres:16-alpine
   - Porta: 5432
   - Volume persistente para dados

### Rede

Todos os serviços estão conectados na rede `consulta-creditos-network` (bridge).

## Estrutura de arquivos Docker

```
consulta-creditos-api/
├── Dockerfile              # Build multi-stage da API
├── docker-compose.yml      # Orquestração de serviços
├── .dockerignore          # Arquivos ignorados no build
├── .env.example           # Template de variáveis de ambiente
└── scripts/
    ├── build.ps1          # Build Docker (PowerShell)
    ├── build.sh           # Build Docker (Bash)
    ├── start.ps1          # Iniciar containers (PowerShell)
    ├── start.sh           # Iniciar containers (Bash)
    ├── stop.ps1           # Parar containers (PowerShell)
    ├── stop.sh            # Parar containers (Bash)
    ├── migrations.ps1     # Aplicar migrations (PowerShell)
    └── migrations.sh      # Aplicar migrations (Bash)
```

## Pré-requisitos

1. Docker Desktop instalado e em execução
2. Docker Compose (incluído no Docker Desktop)
3. Azure Service Bus configurado (ou usar emulador local)

## Configuração

### 1. Variáveis de ambiente

Crie um arquivo `.env` na raiz do projeto:

```bash
cp .env.example .env
```

Configure as seguintes variáveis:

```env
SERVICEBUS_CONNECTION_STRING=Endpoint=sb://your-namespace.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=your-key
```

### 2. Azure Service Bus

Você precisa configurar o Azure Service Bus antes de executar a aplicação:

1. Crie um namespace no Azure Service Bus
2. Crie um tópico chamado `integrar-credito-constituido-entry`
3. Crie uma subscription no tópico
4. Copie a connection string e cole no arquivo `.env`

## Uso

### Opção 1: Scripts automatizados

#### Windows (PowerShell)

```powershell
# Build da imagem
.\scripts\build.ps1

# Iniciar containers
.\scripts\start.ps1

# Aplicar migrations
.\scripts\migrations.ps1

# Parar containers
.\scripts\stop.ps1
```

#### Linux/Mac

```bash
# Build da imagem
./scripts/build.sh

# Iniciar containers
./scripts/start.sh

# Aplicar migrations
./scripts/migrations.sh

# Parar containers
./scripts/stop.sh
```

### Opção 2: Comandos Docker diretos

```bash
# Build da imagem
docker build -t consulta-creditos-api:latest .

# Iniciar todos os serviços
docker-compose up -d

# Ver logs
docker-compose logs -f

# Parar todos os serviços
docker-compose down

# Remover volumes (dados do PostgreSQL)
docker-compose down -v
```

## Dockerfile multi-stage

O Dockerfile utiliza build multi-stage para otimizar o tamanho da imagem:

1. **Stage base**: Runtime da aplicação
2. **Stage build**: SDK para compilação
3. **Stage publish**: Publicação da aplicação
4. **Stage final**: Imagem final otimizada

### Benefícios

- Imagem final menor (apenas runtime)
- Separação de dependências de build e runtime
- Build cache otimizado
- Segurança aprimorada

## Health checks

A aplicação possui health checks configurados:

### API

```bash
curl http://localhost:8080/health/self
curl http://localhost:8080/health/ready
```

### PostgreSQL

Health check automático no docker-compose:
```bash
pg_isready -U postgres -d consulta_creditos
```

## Volumes

### postgres-data

Volume persistente para dados do PostgreSQL.

Para remover (ATENÇÃO: apaga todos os dados):
```bash
docker-compose down -v
```

## Rede

Rede bridge customizada `consulta-creditos-network` permite:
- Comunicação entre containers por nome
- Isolamento de rede
- DNS interno

## Troubleshooting

### Docker Desktop não está rodando

```
ERROR: error during connect: Head "http://%2F%2F.%2Fpipe%2FdockerDesktopLinuxEngine/_ping"
```

**Solução**: Inicie o Docker Desktop

### Porta já em uso

```
Error: bind: address already in use
```

**Solução**: Pare o serviço que está usando a porta ou altere a porta no docker-compose.yml

### Erro ao conectar no PostgreSQL

**Solução**: Aguarde o health check do PostgreSQL ficar saudável

```bash
docker-compose ps
```

### Erro ao conectar no Service Bus

**Solução**: Verifique:
1. Connection string no arquivo `.env`
2. Tópico e subscription criados no Azure
3. Permissões de acesso

### Build lento

**Solução**:
1. Verifique o `.dockerignore` para não copiar arquivos desnecessários
2. Use build cache: `docker build --cache-from consulta-creditos-api:latest`

## Melhores práticas aplicadas

1. **Multi-stage build**: Reduz tamanho da imagem final
2. **.dockerignore**: Evita copiar arquivos desnecessários
3. **Health checks**: Garante disponibilidade dos serviços
4. **Volumes nomeados**: Facilita gerenciamento de dados
5. **Variáveis de ambiente**: Configuração flexível
6. **Restart policies**: Alta disponibilidade
7. **Rede customizada**: Melhor isolamento e DNS

## Comandos úteis

```bash
# Ver logs de um serviço específico
docker-compose logs -f api
docker-compose logs -f postgres

# Executar comando dentro do container
docker-compose exec api bash
docker-compose exec postgres psql -U postgres -d consulta_creditos

# Reconstruir apenas um serviço
docker-compose up -d --build api

# Ver status dos containers
docker-compose ps

# Ver uso de recursos
docker stats

# Limpar recursos não utilizados
docker system prune -a
```

## Ambientes

### Development

```bash
ASPNETCORE_ENVIRONMENT=Development
docker-compose up -d
```

### Production

```bash
ASPNETCORE_ENVIRONMENT=Production
docker-compose -f docker-compose.yml -f docker-compose.prod.yml up -d
```

## Segurança

1. **Não commitar arquivo .env** (incluído no .gitignore)
2. **Usar secrets do Docker para production**
3. **Não expor portas desnecessárias**
4. **Manter imagens atualizadas**
5. **Usar usuário não-root nos containers**

## Próximos passos

1. Criar docker-compose.prod.yml para produção
2. Configurar secrets do Docker Swarm/Kubernetes
3. Adicionar monitoramento com Prometheus/Grafana
4. Implementar log aggregation
5. Configurar CI/CD pipeline
