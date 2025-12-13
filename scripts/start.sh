#!/bin/bash

echo "Verificando arquivo .env..."

if [ ! -f ".env" ]; then
    echo "Arquivo .env nao encontrado. Criando a partir do .env.example..."
    if [ -f ".env.example" ]; then
        cp .env.example .env
        echo "ATENCAO: Configure o arquivo .env com suas credenciais do Azure Service Bus!"
        echo "Pressione Enter para continuar apos configurar o .env..."
        read
    else
        echo "Arquivo .env.example nao encontrado!"
        exit 1
    fi
fi

echo "Iniciando containers com Docker Compose..."

docker-compose up -d

if [ $? -eq 0 ]; then
    echo "Containers iniciados com sucesso!"
    echo ""
    echo "Acesse a API em: http://localhost:8080"
    echo "Health check: http://localhost:8080/health/self"
    echo ""
    echo "Para ver os logs, execute: docker-compose logs -f"
else
    echo "Erro ao iniciar containers!"
    exit 1
fi
