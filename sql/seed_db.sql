drop table if exists railway.TPOTPapers;
CREATE TABLE railway.TPOTPapers (
    id int,
    wordpress_id int,
    author varchar(255),
    category varchar(255),
    link varchar(255),
    excerpt TEXT,
    markdown TEXT,
    frontmatter TEXT,
    RawHtml LONGTEXT,
    RawJson JSON
);

insert into railway.TPOTPapers 
values   (1, 2, 10, "blah", "blah", "blah",  "blah",  "blah",  "blah", "{}")
        ,(1, 2, 10, "blah", "blah", "blah",  "blah",  "blah",  "blah", "{}")
        ,(1, 2, 10, "blah", "blah", "blah",  "blah",  "blah",  "blah", "{}")
        ,(1, 2, 10, "blah", "blah", "blah",  "blah",  "blah",  "blah", "{}")
        ,(1, 2, 10, "blah", "blah", "blah",  "blah",  "blah",  "blah", "{}")
  ;

select * from railway.TPOTPapers;



drop table if exists railway.FacebookComments;
CREATE TABLE railway.FacebookComments (
    id int,
    CssSelector TEXT,
    RawHtml LONGTEXT,
    RawJson JSON
);


