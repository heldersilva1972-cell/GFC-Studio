import pyodbc

conn_str = r'DRIVER={ODBC Driver 17 for SQL Server};SERVER=.\SQLEXPRESS;DATABASE=ClubMembership;Trusted_Connection=yes;'
conn = pyodbc.connect(conn_str)
cursor = conn.cursor()

member_id = 114
test_date = '2024-03-07'

print(f"Updating Member {member_id} AcceptedDate to {test_date}...")
cursor.execute("UPDATE Members SET AcceptedDate = ? WHERE MemberID = ?", (test_date, member_id))
conn.commit()

cursor.execute("SELECT AcceptedDate FROM Members WHERE MemberID = ?", (member_id,))
row = cursor.fetchone()
print(f"After Update, AcceptedDate in DB is: {row[0]}")

cursor.close()
conn.close()
