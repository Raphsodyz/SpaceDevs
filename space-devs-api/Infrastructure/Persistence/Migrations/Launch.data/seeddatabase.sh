#!/bin/bash

migrations_path=$"migrations"
seed_path=$"migrations/seed"
seed_file="spacedevs_data.sql"

if [ -f "/$seed_path/$seed_file" ]; then
    echo 'Found a backup file, recovering the seed dump file...'
    
    psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" < "/$seed_path/$seed_file"
    echo 'The data has recovered by your pg_dump() file.'
else
    echo 'Recovering the migration file...'
    migration_file=$"migrations.sql"

    psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" < "/$migrations_path/$migration_file"
    echo 'The migrations has done.'
fi