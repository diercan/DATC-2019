package com.example.dadproject;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import com.android.volley.AuthFailureError;
import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonObjectRequest;
import com.android.volley.toolbox.Volley;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.HashMap;
import java.util.Map;

public class MainActivity extends AppCompatActivity {

    private TextView mTextCnp;
    public Integer  status;
    public String state = "";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        Button btnVote = findViewById(R.id.buttonSendOtp);

        mTextCnp = (TextView) findViewById(R.id.editTextPassword);


        String url = "https://0cxr9anbqk.execute-api.eu-central-1.amazonaws.com/dev/check-voting-state";
        final RequestQueue requestQueue = Volley.newRequestQueue(this);
        JsonObjectRequest objectRequest = new JsonObjectRequest(
                Request.Method.GET,
                url,
                null,
                new Response.Listener<JSONObject>() {
                    @Override
                    public void onResponse(JSONObject response) {
                        try {
                            String s = response.get("body").toString();
                            JSONArray jsonArray = new JSONArray(s);
                            for(int i = 0; i< jsonArray.length(); i++){
                                JSONObject checkState = (JSONObject)jsonArray.get(i);
                                state = checkState.get("State").toString();
                            }
                        } catch (JSONException e) {
                            e.printStackTrace();
                        }
                    }
                },
                new Response.ErrorListener() {
                    @Override
                    public void onErrorResponse(VolleyError error) {
                        Log.d("Eroarea: ", error.toString());
                    }
                }
        );
        requestQueue.add(objectRequest);

        btnVote.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent i = getIntent();
                finish();
                startActivity(i);
                if(mTextCnp.getText().toString().isEmpty())
                {
                    mTextCnp.setError("Vă rugăm introduceți CNP-ul!");
                    mTextCnp.requestFocus();
                }
                if(state.equals("true")){
                    post();
                }
                else
                {
                    Toast.makeText(MainActivity.this, "Votul nu a început! Vă rugăm încercați mai târziu!", Toast.LENGTH_SHORT).show();
                }
            }
        });
    }

    public void post() {
        JSONObject js = new JSONObject();
        try {

            js.put("CNP", mTextCnp.getText().toString());
        } catch (JSONException e) {
            e.printStackTrace();
        }
        String url = "https://0cxr9anbqk.execute-api.eu-central-1.amazonaws.com/dev/check-voter";
        JsonObjectRequest jsonObjectRequest = new JsonObjectRequest(Request.Method.POST,
                url, js,
                new Response.Listener<JSONObject>() {
                    @Override
                    public void onResponse(JSONObject response) {
                        try {
                            status = Integer.parseInt(response.getString("statusCode"));
                            //Toast.makeText(MainActivity.this, status.toString(), Toast.LENGTH_SHORT).show();
                            if(status == 200){
                                Intent i = new Intent(MainActivity.this, VoteActivity.class);
                                i.putExtra("mCnpSend", mTextCnp.getText().toString());
                                startActivity(i);
                                Toast.makeText(MainActivity.this, "Login reușit!", Toast.LENGTH_SHORT).show();
                            }
                            else if (status == 400)
                            {
                                Toast.makeText(MainActivity.this, "Nu există acest CNP ori a fost folosit înainte!" +"\n" + "Ne pare rău", Toast.LENGTH_SHORT).show();
                            }
                            else
                            {
                                Toast.makeText(MainActivity.this, "Error Server", Toast.LENGTH_SHORT).show();
                            }
                        } catch (JSONException e) {
                            e.printStackTrace();
                        }
                    }
                },
                new Response.ErrorListener() {
                    @Override
                    public void onErrorResponse(VolleyError error) {
                        Toast.makeText(MainActivity.this, error.toString(), Toast.LENGTH_SHORT).show();
                    }
                }) {
            public Map<String, String> getHeaders() throws AuthFailureError {
                HashMap<String, String> headers = new HashMap<String, String>();
                headers.put("Content-Type", "application/json; charset=utf-8");
                return headers;
            }
        };

        Volley.newRequestQueue(this).add(jsonObjectRequest);
    }
}

