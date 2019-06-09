CREATE TABLE test_case_t
(
  id serial NOT NULL,
  test_case_no character varying(30) NOT NULL,
  name character varying(100) NOT NULL,
　test_url text,
  description text,
  create_at timestamp with time zone NOT NULL DEFAULT now(),
  update_at timestamp with time zone,
  CONSTRAINT test_case_t_pkey PRIMARY KEY (id)
)
WITH (
  OIDS=FALSE
);

COMMENT ON COLUMN test_case_t.id IS 'CHK_TYPE_Numeric';
COMMENT ON COLUMN test_case_t.test_case_no IS 'range to 10 CHK_TYPE_NumericString';
COMMENT ON COLUMN test_case_t.name IS 'CHK_TYPE_All';
COMMENT ON COLUMN test_case_t.test_url IS 'CHK_TYPE_All';
COMMENT ON COLUMN test_case_t.description IS 'CHK_TYPE_APP';
COMMENT ON COLUMN test_case_t.create_at IS 'CHK_TYPE_Datetime';
COMMENT ON COLUMN test_case_t.update_at IS 'CHK_TYPE_Datetime';

ALTER TABLE test_case_t OWNER TO postgres;
CREATE INDEX test_case_t_id_idx ON test_case_t USING btree (id);


CREATE TABLE test_case_detail_t
(
  id serial NOT NULL,
  test_case_no character varying(30) NOT NULL,
  elem_no character varying(5) NOT NULL,
  elem_name character varying(1000) NOT NULL,
  sendkey character varying(100) NOT NULL,
  sleep_time character varying(5) NOT NULL,
  create_at timestamp with time zone NOT NULL DEFAULT now(),
  update_at timestamp with time zone,
  CONSTRAINT test_case_detail_t_pkey PRIMARY KEY (id)
)
WITH (
  OIDS=FALSE
);

COMMENT ON COLUMN test_case_detail_t.id IS 'CHK_TYPE_Numeric';
COMMENT ON COLUMN test_case_detail_t.test_case_no IS 'range to 10 CHK_TYPE_NumericString';
COMMENT ON COLUMN test_case_detail_t.elem_no IS 'CHK_TYPE_All';
COMMENT ON COLUMN test_case_detail_t.elem_name IS 'CHK_TYPE_All';
COMMENT ON COLUMN test_case_detail_t.sendkey IS 'CHK_TYPE_All';
COMMENT ON COLUMN test_case_detail_t.sleep_time IS 'renge to 5 CHK_TYPE_Integer';
COMMENT ON COLUMN test_case_detail_t.create_at IS 'CHK_TYPE_Datetime';
COMMENT ON COLUMN test_case_detail_t.update_at IS 'CHK_TYPE_Datetime';

ALTER TABLE test_case_detail_t OWNER TO postgres;
CREATE INDEX test_case_detail_t_id_idx ON test_case_detail_t USING btree (id);