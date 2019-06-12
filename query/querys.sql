--
--追加
alter table test_case_t ADD COLUMN delete integer DEFAULT 0 NOT NULL;
alter table test_case_detail_t ADD COLUMN execute_order character varying(5) DEFAULT '00000' NOT NULL;
--更新
alter table test_case_detail_t alter test_case_no type character varying(30);
alter table test_case_detail_t alter sleep_time type character varying(5);
alter table test_case_detail_t alter sleep_time type character varying(5);
--
INSERT INTO test_case_t(test_case_no, name, test_url, description) VALUES
('demo-001', 'テストデモ', 'https://news.google.com/?hl=ja&gl=JP&ceid=JP:ja', ''),
('demo-002', 'テストデモ', 'https://news.google.com/?hl=ja&gl=JP&ceid=JP:ja', '');

select * from test_case_t;
delete from test_case_t where test_case_no like 'demo%';
SELECT * FROM test_case_t WHERE test_case_no like 'demo%';

INSERT INTO test_case_detail_t(test_case_no, elem_no, elem_name, sendkey, sleep_time) VALUES
('demo-001', '1', '', '', ''),
('demo-001', '205', 'その他の「ヘッドライン」', '', ''),
('demo-001', '1', '', '', '3000'),
('demo-002', '1', '', '', ''),
('demo-002', '205', 'その他の「ヘッドライン」', '', ''),
('demo-002', '1', '', '', '3000');

select * from test_case_detail_t;
delete from test_case_detail_t where test_case_no like 'demo%'
