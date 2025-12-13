#!/bin/bash

echo "Parando containers..."

docker-compose down

if [ $? -eq 0 ]; then
    echo "Containers parados com sucesso!"
else
    echo "Erro ao parar containers!"
    exit 1
fi
