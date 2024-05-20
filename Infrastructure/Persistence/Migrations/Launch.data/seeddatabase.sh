#!/bin/bash

currently_dir=$(dirname "$0")
migrations_path=$"migrations"
seed_path=$"migrations/seed"
seed_file="spacedevs_data.sql"

if [ -f "$currently_dir/../$seed_path/$seed_file" ]; then
    echo 'Found a backup file, recovering the seed dump file...'
    
    chmod 777 "$currently_dir/../$seed_path/$seed_file"
    chown "$POSTGRES_USER":"$POSTGRES_USER" "$currently_dir/../$seed_path/$seed_file"
    psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" < "$currently_dir/../$seed_path/$seed_file"

    echo 'The data has recovered by your pg_dump() file.'
else
    echo 'Recovering the migration file...'
    migration_file=$"migrations.sql"

    chmod 777 "$currently_dir/../$migrations_path/$migration_file"
    chown "$POSTGRES_USER":"$POSTGRES_USER" "$currently_dir/../$migrations_path/$migration_file"
    psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" < "$currently_dir/../$migrations_path/$migration_file"

    echo 'The migrations has done.'
fi