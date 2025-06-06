--
-- PostgreSQL database dump
--

-- Dumped from database version 16.3
-- Dumped by pg_dump version 16.3

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

--
-- Name: contracts; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.contracts (
    id integer NOT NULL,
    name character varying(255),
    date_of_conclusion date,
    amount numeric(10,2),
    customers_id integer
);


ALTER TABLE public.contracts OWNER TO postgres;

--
-- Name: contracts_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.contracts_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.contracts_id_seq OWNER TO postgres;

--
-- Name: contracts_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.contracts_id_seq OWNED BY public.contracts.id;


--
-- Name: customers; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.customers (
    id integer NOT NULL,
    last_name character varying(200),
    first_name character varying(200),
    patronymic character varying(200),
    organization_name character varying(255),
    email character varying(255),
    phone numeric(11,0)
);


ALTER TABLE public.customers OWNER TO postgres;

--
-- Name: customers_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.customers_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.customers_id_seq OWNER TO postgres;

--
-- Name: customers_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.customers_id_seq OWNED BY public.customers.id;


--
-- Name: employees; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.employees (
    id integer NOT NULL,
    last_name character varying(255),
    first_name character varying(255),
    patronymic character varying(255),
    "position" character varying(100),
    salary numeric(10,2),
    username character varying(50) NOT NULL,
    password character varying(100) NOT NULL
);


ALTER TABLE public.employees OWNER TO postgres;

--
-- Name: employees_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.employees_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.employees_id_seq OWNER TO postgres;

--
-- Name: employees_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.employees_id_seq OWNED BY public.employees.id;


--
-- Name: equipment; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.equipment (
    id integer NOT NULL,
    name character varying(255),
    date_of_purchase date,
    cost numeric(10,2),
    supplier_id integer
);


ALTER TABLE public.equipment OWNER TO postgres;

--
-- Name: equipment_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.equipment_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.equipment_id_seq OWNER TO postgres;

--
-- Name: equipment_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.equipment_id_seq OWNED BY public.equipment.id;


--
-- Name: materials; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.materials (
    id integer NOT NULL,
    name character varying(255),
    quantity numeric,
    price numeric(10,2),
    supplier_id integer
);


ALTER TABLE public.materials OWNER TO postgres;

--
-- Name: materials_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.materials_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.materials_id_seq OWNER TO postgres;

--
-- Name: materials_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.materials_id_seq OWNED BY public.materials.id;


--
-- Name: payments; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.payments (
    id integer NOT NULL,
    amount numeric(10,2),
    type character varying(100),
    date date,
    contract_id integer,
    CONSTRAINT payments_type_check CHECK (((type)::text = ANY (ARRAY[('Terminal'::character varying)::text, ('Cash'::character varying)::text])))
);


ALTER TABLE public.payments OWNER TO postgres;

--
-- Name: payments_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.payments_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.payments_id_seq OWNER TO postgres;

--
-- Name: payments_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.payments_id_seq OWNED BY public.payments.id;


--
-- Name: projects; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.projects (
    id integer NOT NULL,
    name character varying(255),
    description text,
    start_date timestamp without time zone,
    end_date timestamp without time zone,
    customers_id integer
);


ALTER TABLE public.projects OWNER TO postgres;

--
-- Name: projects_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.projects_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.projects_id_seq OWNER TO postgres;

--
-- Name: projects_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.projects_id_seq OWNED BY public.projects.id;


--
-- Name: suppliers; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.suppliers (
    id integer NOT NULL,
    organization_name character varying(255),
    email character varying(255),
    phone numeric(11,0)
);


ALTER TABLE public.suppliers OWNER TO postgres;

--
-- Name: suppliers_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.suppliers_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.suppliers_id_seq OWNER TO postgres;

--
-- Name: suppliers_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.suppliers_id_seq OWNED BY public.suppliers.id;


--
-- Name: tasks; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tasks (
    id integer NOT NULL,
    description text,
    status character varying(50),
    start_date date,
    end_date date,
    project_id integer,
    employee_id integer
);


ALTER TABLE public.tasks OWNER TO postgres;

--
-- Name: tasks_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.tasks_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.tasks_id_seq OWNER TO postgres;

--
-- Name: tasks_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.tasks_id_seq OWNED BY public.tasks.id;


--
-- Name: contracts id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.contracts ALTER COLUMN id SET DEFAULT nextval('public.contracts_id_seq'::regclass);


--
-- Name: customers id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.customers ALTER COLUMN id SET DEFAULT nextval('public.customers_id_seq'::regclass);


--
-- Name: employees id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.employees ALTER COLUMN id SET DEFAULT nextval('public.employees_id_seq'::regclass);


--
-- Name: employees username; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.employees ALTER COLUMN username SET DEFAULT ('user'::text || nextval('public.employees_id_seq'::regclass));


--
-- Name: employees password; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.employees ALTER COLUMN password SET DEFAULT ('pass'::text || nextval('public.employees_id_seq'::regclass));


--
-- Name: equipment id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.equipment ALTER COLUMN id SET DEFAULT nextval('public.equipment_id_seq'::regclass);


--
-- Name: materials id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.materials ALTER COLUMN id SET DEFAULT nextval('public.materials_id_seq'::regclass);


--
-- Name: payments id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.payments ALTER COLUMN id SET DEFAULT nextval('public.payments_id_seq'::regclass);


--
-- Name: projects id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.projects ALTER COLUMN id SET DEFAULT nextval('public.projects_id_seq'::regclass);


--
-- Name: suppliers id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.suppliers ALTER COLUMN id SET DEFAULT nextval('public.suppliers_id_seq'::regclass);


--
-- Name: tasks id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tasks ALTER COLUMN id SET DEFAULT nextval('public.tasks_id_seq'::regclass);


--
-- Data for Name: contracts; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.contracts (id, name, date_of_conclusion, amount, customers_id) FROM stdin;
6	Договор на строительство жилого комплекса	2022-10-20	2500000.00	1
7	Договор на ремонт дороги	2022-11-30	800000.00	2
8	Договор на строительство торгового центра	2023-01-15	5000000.00	3
9	Договор на реконструкцию парка	2023-02-25	1500000.00	1
10	Договор на строительство жилого дома	2023-03-30	3000000.00	2
11	Договор на строительство жилого комплекса	2022-10-20	2500000.00	1
12	Договор на ремонт дороги	2022-11-30	800000.00	2
13	Договор на строительство торгового центра	2023-01-15	5000000.00	3
14	Договор на реконструкцию парка	2023-02-25	1500000.00	1
15	Договор на строительство жилого дома	2023-03-30	3000000.00	2
\.


--
-- Data for Name: customers; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.customers (id, last_name, first_name, patronymic, organization_name, email, phone) FROM stdin;
1	Попов	Иван	Алексеевич	ООО "СтройИнвест"	popov@example.com	123456789
2	Козлов	Дмитрий	Андреевич	Городская администрация	kozlov@example.com	987654321
3	Морозов	Александр	Владимирович	ООО "ЭкоСтрой"	morozov@example.com	111222333
4	Ковалева	Елена	Александровна	ИП Ковалева	kovalyova@example.com	444555666
5	Новиков	Сергей	Иванович	ООО "СкайЛайн"	novikov@example.com	777888999
6	Попов	Иван	Алексеевич	ООО "СтройИнвест"	popov@example.com	123456789
7	Козлов	Дмитрий	Андреевич	Городская администрация	kozlov@example.com	987654321
8	Морозов	Александр	Владимирович	ООО "ЭкоСтрой"	morozov@example.com	111222333
9	Ковалева	Елена	Александровна	ИП Ковалева	kovalyova@example.com	444555666
10	Новиков	Сергей	Иванович	ООО "СкайЛайн"	novikov@example.com	777888999
11	Попов	Иван	Алексеевич	ООО "СтройИнвест"	popov@example.com	123456789
12	Козлов	Дмитрий	Андреевич	Городская администрация	kozlov@example.com	987654321
13	Морозов	Александр	Владимирович	ООО "ЭкоСтрой"	morozov@example.com	111222333
14	Ковалева	Елена	Александровна	ИП Ковалева	kovalyova@example.com	444555666
15	Новиков	Сергей	Иванович	ООО "СкайЛайн"	novikov@example.com	777888999
\.


--
-- Data for Name: employees; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.employees (id, last_name, first_name, patronymic, "position", salary, username, password) FROM stdin;
2	Смирнова	Елена	Александровна	Архитектор	70000.00	user103	pass104
3	Петров	Дмитрий	Иванович	Прораб	55000.00	user105	pass106
4	Козлова	Ольга	Дмитриевна	Бухгалтер	45000.00	user107	pass108
5	Сидоров	Андрей	Сергеевич	Менеджер по закупкам	50000.00	user109	pass110
16	Иванов	Александр	Петрович	Инженер-строитель	60000.00	user111	pass112
17	Смирнова	Елена	Александровна	Архитектор	70000.00	user113	pass114
18	Петров	Дмитрий	Иванович	Прораб	55000.00	user115	pass116
19	Козлова	Ольга	Дмитриевна	Бухгалтер	45000.00	user117	pass118
20	Сидоров	Андрей	Сергеевич	Менеджер по закупкам	50000.00	user119	pass120
21	Иванов	Александр	Петрович	Инженер-строитель	60000.00	user121	pass122
22	Смирнова	Елена	Александровна	Архитектор	70000.00	user123	pass124
23	Петров	Дмитрий	Иванович	Прораб	55000.00	user125	pass126
24	Козлова	Ольга	Дмитриевна	Бухгалтер	45000.00	user127	pass128
25	Сидоров	Андрей	Сергеевич	Менеджер по закупкам	50000.00	user129	pass130
1	Иванов	Александр	Петрович	Admin	60000.00	user101	pass102
\.


--
-- Data for Name: equipment; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.equipment (id, name, date_of_purchase, cost, supplier_id) FROM stdin;
21	Экскаватор	2022-10-15	15000.00	1
22	Бетоносмеситель	2022-09-20	12000.00	2
24	Бульдозер	2022-12-05	25000.00	1
25	Виброплита	2023-01-20	8000.00	2
23	Автокран	2022-11-10	18000.00	2
\.


--
-- Data for Name: materials; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.materials (id, name, quantity, price, supplier_id) FROM stdin;
6	Цемент	5000	40.00	1
7	Арматура	2000	30.00	2
8	Песок	8000	20.00	3
9	Бетон	7000	50.00	1
10	Керамическая плитка	3000	25.00	2
11	Цемент	5000	40.00	1
12	Арматура	2000	30.00	2
13	Песок	8000	20.00	3
14	Бетон	7000	50.00	1
15	Керамическая плитка	3000	25.00	2
\.


--
-- Data for Name: payments; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.payments (id, amount, type, date, contract_id) FROM stdin;
\.


--
-- Data for Name: projects; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.projects (id, name, description, start_date, end_date, customers_id) FROM stdin;
1	Строительство школы №1	Строительство новой средней школы в районе "Парковый"	2022-11-01 00:00:00	2023-07-31 00:00:00	1
2	Ремонт дороги по ул. Центральная	Полное восстановление дорожного покрытия на протяжении 2 км	2022-12-15 00:00:00	2023-05-31 00:00:00	2
3	Строительство жилого комплекса "Зеленая поляна"	Возводится многоэтажный жилой дом с закрытой территорией	2023-01-10 00:00:00	2024-01-10 00:00:00	3
4	Реконструкция парка "Лесная поляна"	Обновление парковой инфраструктуры, строительство детских площадок и спортивных площадок	2023-02-20 00:00:00	2023-12-31 00:00:00	1
6	Строительство школы №1	Строительство новой средней школы в районе "Парковый"	2022-11-01 00:00:00	2023-07-31 00:00:00	1
7	Ремонт дороги по ул. Центральная	Полное восстановление дорожного покрытия на протяжении 2 км	2022-12-15 00:00:00	2023-05-31 00:00:00	2
8	Строительство жилого комплекса "Зеленая поляна"	Возводится многоэтажный жилой дом с закрытой территорией	2023-01-10 00:00:00	2024-01-10 00:00:00	3
9	Реконструкция парка "Лесная поляна"	Обновление парковой инфраструктуры, строительство детских площадок и спортивных площадок	2023-02-20 00:00:00	2023-12-31 00:00:00	1
10	Строительство торгового центра "Мегаполис"	Строительство крупного торгового центра с современным дизайном	2023-03-25 00:00:00	2024-06-30 00:00:00	5
5	Строительство торгового центра "Мегаполис"	Строительство крупного торгового центра с современным дизайном	2023-03-25 00:00:00	2024-06-30 00:00:00	7
\.


--
-- Data for Name: suppliers; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.suppliers (id, organization_name, email, phone) FROM stdin;
1	ООО "СтройМатериалы"	info@stroymaterials.com	111222333
2	ЗАО "СтройТехника"	info@stroytechnika.com	444555666
3	ООО "Бетонные решения"	info@betonnyeresheniya.com	777888999
4	ИП "СтройРесурсы"	info@stroyresurs.com	222333444
5	ООО "МатериалСтрой"	info@materialstroy.com	555666777
6	ООО "СтройМатериалы"	info@stroymaterials.com	111222333
7	ЗАО "СтройТехника"	info@stroytechnika.com	444555666
8	ООО "Бетонные решения"	info@betonnyeresheniya.com	777888999
9	ИП "СтройРесурсы"	info@stroyresurs.com	222333444
10	ООО "МатериалСтрой"	info@materialstroy.com	555666777
\.


--
-- Data for Name: tasks; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.tasks (id, description, status, start_date, end_date, project_id, employee_id) FROM stdin;
46	Разработка проектной документации	В работе	2022-11-01	2022-12-15	1	2
48	Монтаж каркаса	В работе	2022-12-01	2023-01-15	1	3
49	Подготовка площадки для асфальтирования	Завершено	2022-12-15	2022-12-31	2	3
47	Устройство фундамента	В процессе	2022-11-10	2022-11-30	1	3
50	Устройство асфальтового покрытия	Завершена	2023-01-01	2023-05-31	2	3
\.


--
-- Name: contracts_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.contracts_id_seq', 15, true);


--
-- Name: customers_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.customers_id_seq', 17, true);


--
-- Name: employees_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.employees_id_seq', 130, true);


--
-- Name: equipment_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.equipment_id_seq', 26, true);


--
-- Name: materials_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.materials_id_seq', 16, true);


--
-- Name: payments_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.payments_id_seq', 6, true);


--
-- Name: projects_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.projects_id_seq', 29, true);


--
-- Name: suppliers_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.suppliers_id_seq', 11, true);


--
-- Name: tasks_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.tasks_id_seq', 50, true);


--
-- Name: contracts contracts_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.contracts
    ADD CONSTRAINT contracts_pkey PRIMARY KEY (id);


--
-- Name: customers customers_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.customers
    ADD CONSTRAINT customers_pkey PRIMARY KEY (id);


--
-- Name: employees employees_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.employees
    ADD CONSTRAINT employees_pkey PRIMARY KEY (id);


--
-- Name: equipment equipment_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.equipment
    ADD CONSTRAINT equipment_pkey PRIMARY KEY (id);


--
-- Name: materials materials_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.materials
    ADD CONSTRAINT materials_pkey PRIMARY KEY (id);


--
-- Name: payments payments_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.payments
    ADD CONSTRAINT payments_pkey PRIMARY KEY (id);


--
-- Name: projects projects_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.projects
    ADD CONSTRAINT projects_pkey PRIMARY KEY (id);


--
-- Name: suppliers suppliers_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.suppliers
    ADD CONSTRAINT suppliers_pkey PRIMARY KEY (id);


--
-- Name: tasks tasks_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tasks
    ADD CONSTRAINT tasks_pkey PRIMARY KEY (id);


--
-- Name: contracts contracts_customers_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.contracts
    ADD CONSTRAINT contracts_customers_id_fkey FOREIGN KEY (customers_id) REFERENCES public.customers(id);


--
-- Name: equipment equipment_supplier_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.equipment
    ADD CONSTRAINT equipment_supplier_id_fkey FOREIGN KEY (supplier_id) REFERENCES public.suppliers(id);


--
-- Name: materials materials_supplier_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.materials
    ADD CONSTRAINT materials_supplier_id_fkey FOREIGN KEY (supplier_id) REFERENCES public.suppliers(id);


--
-- Name: payments payments_contract_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.payments
    ADD CONSTRAINT payments_contract_id_fkey FOREIGN KEY (contract_id) REFERENCES public.contracts(id);


--
-- Name: projects projects_customers_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.projects
    ADD CONSTRAINT projects_customers_id_fkey FOREIGN KEY (customers_id) REFERENCES public.customers(id);


--
-- Name: tasks tasks_employee_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tasks
    ADD CONSTRAINT tasks_employee_id_fkey FOREIGN KEY (employee_id) REFERENCES public.employees(id);


--
-- Name: tasks tasks_project_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tasks
    ADD CONSTRAINT tasks_project_id_fkey FOREIGN KEY (project_id) REFERENCES public.projects(id);


--
-- PostgreSQL database dump complete
--

