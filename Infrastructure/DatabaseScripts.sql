create database if not exists monito;
use monito;

create table if not exists users (
	id int not null auto_increment primary key,
	name varchar(100),
	email varchar(100),
	password varchar(100),
	ip varchar(100),
	uuid varchar(100) unique,
    created_at datetime,
    updated_at datetime
);

create table if not exists requests (
	id int not null auto_increment primary key,
    type tinyint default 1, -- Simple: 1, Batch: 2
    options longtext,
    status tinyint default 0, -- Incomplete: 0, Ready: 1, InProgress: 2, Done: 3
    uuid varchar(100) unique,
    created_at datetime,
    updated_at datetime,
    user_id int,
    foreign key (user_id) references users(id)
);

create table if not exists links (
	id int not null auto_increment primary key,
    url varchar(2048),
    status tinyint default 0, -- Idle: 0, InProgress: 1, Done: 2
    output varchar(100),
    status_code int,
    additional_data longtext,
    uuid varchar(100) unique,
    created_at datetime,
    updated_at datetime,
    redirects_from_link_id int null,
    request_id int,
    foreign key (redirects_from_link_id) references links(id),
    foreign key (request_id) references requests(id)
);

create table if not exists files (
	id int not null auto_increment primary key,
    name varchar(2048),
    type tinyint, -- TXT: 1, CSV: 2
    created_at datetime,
    updated_at datetime,
    request_id int,
    foreign key (request_id) references requests(id)
);

create table if not exists workers (
	id int not null auto_increment primary key,
    hostname varchar(1000),
    uuid varchar(100) unique,
    ping datetime,
    created_at datetime,
    updated_at datetime
);

create table if not exists queues (
	id int not null auto_increment primary key,
    type tinyint default 1, -- Basic: 1, Priority: 2, Simple: 3
    status tinyint default 0, -- Idle: 0, Busy: 1, Offline: 2
    uuid varchar(100) unique,
    created_at datetime,
    updated_at datetime,
    worker_id int,
    foreign key (worker_id) references workers(id)
);