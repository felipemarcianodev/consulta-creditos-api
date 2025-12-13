#!/bin/bash

echo "Iniciando build da imagem Docker..."

docker build -t consulta-creditos-api:latest .

if [ $? -eq 0 ]; then
    echo "Build concluido com sucesso!"
else
    echo "Erro no build!"
    exit 1
fi
