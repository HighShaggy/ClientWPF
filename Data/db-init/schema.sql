CREATE TABLE business_areas (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL
);

CREATE TABLE clients (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    inn VARCHAR(12) NOT NULL,
    business_area_id INT REFERENCES business_areas(id),
    note TEXT
);

CREATE TABLE request_statuses (
    id SERIAL PRIMARY KEY,
    name VARCHAR(50) NOT NULL
);

CREATE TABLE requests (
    id SERIAL PRIMARY KEY,
    client_id INT REFERENCES clients(id) ON DELETE CASCADE,
    request_date DATE NOT NULL,
    work_name VARCHAR(255) NOT NULL,
    work_description TEXT,
    status_id INT REFERENCES request_statuses(id)
);

CREATE INDEX idx_clients_name ON clients(name);
CREATE INDEX idx_requests_date ON requests(request_date);
CREATE INDEX idx_requests_client ON requests(client_id);

