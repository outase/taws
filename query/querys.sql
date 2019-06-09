--
alter table test_case_detail_t alter test_case_no type character varying(30);
alter table test_case_detail_t alter sleep_time type character varying(5);

--
INSERT INTO test_case_t(test_case_no, name, test_url, description) VALUES
('demo001', 'テストデモ', 'https://news.google.com/?hl=ja&gl=JP&ceid=JP:ja', ''),
('demo002', 'テストデモ', 'https://news.google.com/?hl=ja&gl=JP&ceid=JP:ja', '');

select * from test_case_t;
delete from test_case_t where id = '3'
SELECT COUNT(*) FROM test_case_t WHERE SUBSTRING('test_case_no', 0, 7) = 'demo001';

INSERT INTO test_case_detail_t(test_case_no, elem_no, elem_name, sendkey, sleep_time) VALUES
('demo001', '1', '', '', ''),
('demo001', '205', 'その他の「ヘッドライン」', '', ''),
('demo001', '1', '', '', '3000'),
('demo002', '1', '', '', ''),
('demo002', '205', 'その他の「ヘッドライン」', '', ''),
('demo002', '1', '', '', '3000');

select * from test_case_detail_t;
delete from test_case_detail_t where test_case_no = 'demo001'
