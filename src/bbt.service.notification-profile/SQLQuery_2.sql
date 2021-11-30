
UPDATE [Consumers] SET Email = 'zstring' 
WHERE Email = 'xstring' AND 
      [User] = 123456
      --User = CAST('123456' AS bigint)