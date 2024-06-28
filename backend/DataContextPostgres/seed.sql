CREATE TABLE url
(
    id SERIAL PRIMARY KEY,
    hash VARCHAR(10),
    original_url TEXT NOT NULL UNIQUE,
    created_at TIMESTAMP,
    access_count INT DEFAULT 0
);

INSERT INTO url(hash, original_url, created_at)
VALUES 
    ('a', 'https://www.youtube.com/watch?v=l_7JA-4UVvI&t=2542s&ab_channel=Jordaan', CURRENT_TIMESTAMP),
    ('b', 'https://soundcloud.com/ivangogh', CURRENT_TIMESTAMP);
