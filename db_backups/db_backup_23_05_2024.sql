PGDMP  .    6                 |            web_project_users    16.2    16.2 	    �           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            �           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            �           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            �           1262    16472    web_project_users    DATABASE     �   CREATE DATABASE web_project_users WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'Russian_Russia.1251';
 !   DROP DATABASE web_project_users;
                postgres    false            �            1259    16474    users    TABLE     �   CREATE TABLE public.users (
    id bigint NOT NULL,
    username character varying(32) NOT NULL,
    password character varying(32) NOT NULL,
    cart character varying(1024) NOT NULL,
    end_date date NOT NULL
);
    DROP TABLE public.users;
       public         heap    postgres    false            �            1259    16473    users_id_seq    SEQUENCE     �   ALTER TABLE public.users ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.users_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    216            �          0    16474    users 
   TABLE DATA           G   COPY public.users (id, username, password, cart, end_date) FROM stdin;
    public          postgres    false    216   �       �           0    0    users_id_seq    SEQUENCE SET     ;   SELECT pg_catalog.setval('public.users_id_seq', 71, true);
          public          postgres    false    215            Q           2606    16483    users users_pkey 
   CONSTRAINT     N   ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (id);
 :   ALTER TABLE ONLY public.users DROP CONSTRAINT users_pkey;
       public            postgres    false    216            �   �   x�m�;�0�z}� Ȼ���"Q�@z�Hq��D�A&�������;�4w�v���X1I��45ja� ��Q�\���0T��`ߥ���#)]��:��Ć-�Y�����I�����)� �J{�N����4Bq�:���s��%�b��~o�8��O�&5     