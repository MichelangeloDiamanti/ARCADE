using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.SqliteClient;

public class DatabaseConnection : MonoBehaviour
{

    private string _constr = "URI=file:sqlite3.db";
    private IDbConnection _dbc;
    private IDbCommand _dbcm;
    private IDataReader _dbr;

    // Use this for initialization
    void Start()
    {
        _dbc = new SqliteConnection(_constr);
        _dbc.Open();
        _dbcm = _dbc.CreateCommand();
        _dbcm.CommandText = "SELECT id FROM people";
        _dbr = _dbcm.ExecuteReader();

        while (_dbr.Read())
        {
             Debug.Log(_dbr.GetInt16(0));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
