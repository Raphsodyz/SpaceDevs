SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;
SET default_tablespace = '';
SET default_table_access_method = heap;

CREATE EXTENSION IF NOT EXISTS pg_trgm WITH SCHEMA public;
COMMENT ON EXTENSION pg_trgm IS 'Text similarity measurement and index searching based on trigrams.';

CREATE TABLE IF NOT EXISTS public.orbit(
    id UUID PRIMARY KEY,
    id_from_api INT NULL,
    atualization_date TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    imported_t TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    status VARCHAR(15) NOT NULL,
    name VARCHAR(255) NULL,
    abbrev VARCHAR(255) NULL
);

CREATE TABLE IF NOT EXISTS public.mission(
    id UUID PRIMARY KEY,
    id_from_api INT NULL,
    atualization_date TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    imported_t TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    status VARCHAR(15) NOT NULL,
    launch_library_id INT NULL,
    name VARCHAR(255) NULL,
    description VARCHAR(2500) NULL,
    type VARCHAR(255) NULL,
    id_orbit UUID NULL,
    launch_designator VARCHAR(255) NULL,
    search VARCHAR(360) GENERATED ALWAYS AS (
        LOWER(name)
    ) STORED NULL,

    CONSTRAINT fk_mission_orbit FOREIGN KEY (id_orbit) REFERENCES public.orbit(id)
);

CREATE TABLE IF NOT EXISTS public.configuration(
    id UUID PRIMARY KEY,
    id_from_api INT NULL,
    atualization_date TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    imported_t TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    status VARCHAR(15) NOT NULL,
    launch_library_id INT NULL,
    url VARCHAR(1000) NULL,
    name VARCHAR(255) NULL,
    family VARCHAR(255) NULL,
    full_name VARCHAR(255) NULL,
    variant VARCHAR(255) NULL,
    search VARCHAR(360) GENERATED ALWAYS AS (
        LOWER(name || ' ' || family)
    ) STORED NULL
);

CREATE TABLE IF NOT EXISTS public.status(
    id UUID PRIMARY KEY,
    id_from_api INT NULL,
    atualization_date TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    imported_t TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    status VARCHAR(15) NOT NULL,
    name VARCHAR(255) NULL,
    abbrev VARCHAR(255) NULL,
    description VARCHAR(2500) NULL
);

CREATE TABLE IF NOT EXISTS public.launch_service_provider(
    id UUID PRIMARY KEY,
    id_from_api INT NULL,
    atualization_date TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    imported_t TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    status VARCHAR(15) NOT NULL,    
    url VARCHAR(1000) NULL,
    name VARCHAR(255) NULL,
    type VARCHAR(255) NULL
);

CREATE TABLE IF NOT EXISTS public.location(
    id UUID PRIMARY KEY,
    id_from_api INT NULL,
    atualization_date TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    imported_t TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    status VARCHAR(15) NOT NULL,
    url VARCHAR(1000) NULL,
    name VARCHAR(255) NULL,
    country_code VARCHAR(255) NULL,
    map_image VARCHAR(1000) NULL,
    total_launch_count INT NULL,
    total_landing_count INT NULL,
    search VARCHAR(360) GENERATED ALWAYS AS (
        LOWER(name)
    ) STORED NULL
);

CREATE TABLE IF NOT EXISTS public.pad(
    id UUID PRIMARY KEY,
    id_from_api INT NULL,
    atualization_date TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    imported_t TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    status VARCHAR(15) NOT NULL,
    url VARCHAR(1000) NULL,
    agency_id INT NULL,
    name VARCHAR(255) NULL,
    info_url VARCHAR(1000) NULL,
    wiki_url VARCHAR(1000) NULL,
    map_url VARCHAR(1000) NULL,
    latitude DOUBLE PRECISION NULL,
    longitude DOUBLE PRECISION NULL,
    id_location UUID NULL,
    map_image VARCHAR(1000) NULL,
    total_launch_count INT NULL,
    search VARCHAR(360) GENERATED ALWAYS AS (
        LOWER(name)
    ) STORED NULL,

    CONSTRAINT fk_pad_location FOREIGN KEY (id_location) REFERENCES public.location(id)
);

CREATE TABLE IF NOT EXISTS public.rocket(
    id UUID PRIMARY KEY,
    id_from_api INT NULL,
    atualization_date TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    imported_t TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    status VARCHAR(15) NOT NULL,
    id_configuration UUID NULL,

    CONSTRAINT fk_rocket_configuration FOREIGN KEY(id_configuration) REFERENCES public.configuration(id)
);

CREATE TABLE IF NOT EXISTS public.launch(
    id UUID PRIMARY KEY,
    id_from_api INT NULL,
    atualization_date TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    imported_t TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    status VARCHAR(15) NOT NULL,
    api_guid UUID NOT NULL,
    url VARCHAR(1000) NOT NULL,
    launch_library_id INT NULL,
    slug VARCHAR(255) NULL,
    name VARCHAR(255) NULL,
    id_status UUID NULL,
    net TIMESTAMP WITHOUT TIME ZONE NULL,
    window_end TIMESTAMP WITHOUT TIME ZONE NULL,
    window_start TIMESTAMP WITHOUT TIME ZONE NULL,
    inhold BOOLEAN NULL,
    tbd_time BOOLEAN NULL,
    tbd_date BOOLEAN NULL,
    probability INT NULL,
    hold_reason VARCHAR(255) NULL,
    fail_reason VARCHAR(255) NULL,
    hashtag VARCHAR(255) NULL,
    id_launch_service_provider UUID NULL,
    id_rocket UUID NULL,
    id_mission UUID NULL,
    id_pad UUID NULL,
    web_cast_live BOOLEAN NULL,
    image VARCHAR(1000) NULL,
    infographic VARCHAR(255) NULL,
    programs VARCHAR(255) NULL,
    search VARCHAR(360) GENERATED ALWAYS AS (
        LOWER(name || ' ' || slug)
    ) STORED NULL,

    CONSTRAINT fk_launch_status FOREIGN KEY (id_status) REFERENCES public.status(id),
    CONSTRAINT fk_launch_launch_service_provider FOREIGN KEY (id_launch_service_provider) REFERENCES public.launch_service_provider(id),
    CONSTRAINT fk_launch_rocket FOREIGN KEY (id_rocket) REFERENCES public.rocket(id),
    CONSTRAINT fk_launch_mission FOREIGN KEY (id_mission) REFERENCES public.mission(id),
    CONSTRAINT fk_launch_pad FOREIGN KEY (id_pad) REFERENCES public.pad(id)
);

CREATE TABLE IF NOT EXISTS public.update_log_routine(
    id UUID PRIMARY KEY,
    id_from_api INT NULL,
    atualization_date TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    imported_t TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    status VARCHAR(15) NOT NULL,
    transaction_date TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    offset_data INT NOT NULL,
    success BOOLEAN NOT NULL,
    message VARCHAR(255) NOT NULL,
    entity_count INT NOT NULL,
    origin VARCHAR(255) NOT NULL
);

CREATE INDEX IDX_GIN_LAUNCH_SLUG_NAME ON public.launch USING GIN (search public.gin_trgm_ops);
CREATE INDEX IDX_GIN_CONFIGURATION_NAME_FAMILY ON public.configuration USING GIN (search public.gin_trgm_ops);
CREATE INDEX IDX_GIN_MISSION_NAME ON public.mission USING GIN (search public.gin_trgm_ops);
CREATE INDEX IDX_GIN_LOCATION_NAME ON public.location USING GIN (search public.gin_trgm_ops);
CREATE INDEX IDX_GIN_PAD_NAME ON public.pad USING GIN (search public.gin_trgm_ops);

SET search_path TO "$user", public;