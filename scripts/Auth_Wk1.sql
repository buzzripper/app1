

SELECT u.UserName, r.Name, c.ClaimType, c.ClaimValue FROM AspNetUsers u
LEFT JOIN AspNetUserClaims c ON
	u.Id = c.UserId
LEFT JOIN AspNetUserRoles ur ON
	ur.UserId = u.Id
LEFT JOIN AspNetRoles r ON
	ur.RoleId = r.Id
ORDER BY u.UserName


SELECT u.UserName, r.Name, rc.ClaimType, rc.ClaimValue FROM AspNetUsers u
INNER JOIN AspNetUserRoles ur ON
	u.Id = ur.UserId
INNER JOIN AspNetRoles r ON
	ur.RoleId = r.Id
INNER JOIN AspNetRoleClaims rc ON
	rc.RoleId = r.Id
ORDER BY u.UserName
