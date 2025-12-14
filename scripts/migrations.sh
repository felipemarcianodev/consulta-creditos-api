#!/bin/bash

echo "Aplicando migrations no banco de dados..."

docker-compose exec consulta-creditos-api dotnet ef database update --project /src/src/ConsultaCreditos.Infrastructure --startup-project /src/src/ConsultaCreditos.API

if [ $? -eq 0 ]; then
    echo "Migrations aplicadas com sucesso!"
else
    echo "Erro ao aplicar migrations!"
    echo "Certifique-se de que os containers estão rodando (execute ./scripts/start.sh primeiro)"
    exit 1
fi
