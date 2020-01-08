package com.example.dadproject;

import android.content.Intent;
import android.os.Build;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.LinearLayout;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.Toast;

import androidx.annotation.RequiresApi;
import androidx.appcompat.app.ActionBar;
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

public class VoteActivity extends AppCompatActivity {

    private String mCnpText;
    String name[];
    String partid[];
    private Button mSendButton, btnLogout;
    String voteType, sendValue;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_vote);

        mCnpText =  getIntent().getStringExtra("mCnpSend");

        mSendButton = findViewById(R.id.button);
        btnLogout = findViewById(R.id.btnLogout);

        //GET METHOD Vote Type
        String url1 = "https://0cxr9anbqk.execute-api.eu-central-1.amazonaws.com/dev/get-voting-type";
        final RequestQueue requestQueue1 = Volley.newRequestQueue(this);
        JsonObjectRequest objectRequest1 = new JsonObjectRequest(
                Request.Method.GET,
                url1,
                null,
                new Response.Listener<JSONObject>() {
                    @Override
                    public void onResponse(JSONObject response) {
                        try {
                            String s = response.get("body").toString();
                            JSONArray jsonArray = new JSONArray(s);
                            name = new String[jsonArray.length()];
                            partid =  new String[jsonArray.length()];
                            for(int i = 0; i< jsonArray.length(); i++){

                                JSONObject type = (JSONObject)jsonArray.get(i);
                                voteType = type.get("TipAlegeri").toString();
                                ActionBar actionBar = getSupportActionBar();
                                if(voteType.equals("1")){
                                    actionBar.setTitle(getString(R.string.parlamentare));
                                }
                                else if(voteType.equals("2")) {
                                    actionBar.setTitle(getString(R.string.prezidentiale));
                                }
                                else if(voteType.equals("3")) {
                                    actionBar.setTitle(getString(R.string.locale));
                                }
                                else if(voteType.equals("4")) {
                                    actionBar.setTitle(getString(R.string.europene));
                                }
                            }
                        } catch (JSONException e) {
                            e.printStackTrace();
                        }
                    }
                },
                new Response.ErrorListener() {
                    @Override
                    public void onErrorResponse(VolleyError error) {
                        Toast.makeText(VoteActivity.this, error.toString(), Toast.LENGTH_SHORT).show();
                    }
                }
        );
        requestQueue1.add(objectRequest1);
        ////////////////////////////

        //GET METHOD Candidati info
        String url = "https://0cxr9anbqk.execute-api.eu-central-1.amazonaws.com/dev/show-info";
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
                            name = new String[jsonArray.length()];
                            partid =  new String[jsonArray.length()];
                            for(int i = 0; i< jsonArray.length(); i++){

                                JSONObject candidate = (JSONObject)jsonArray.get(i);
                                name[i] = candidate.get("NumeCandidat").toString();
                                partid[i] = candidate.get("Partid").toString();
                            }
                            addRadioButtons();
                        } catch (JSONException e) {
                            e.printStackTrace();
                        }
                    }
                },
                new Response.ErrorListener() {
                    @Override
                    public void onErrorResponse(VolleyError error) {
                        Toast.makeText(VoteActivity.this, error.toString(), Toast.LENGTH_SHORT).show();
                    }
                }
        );
        requestQueue.add(objectRequest);

        mSendButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String radioText;
                RadioGroup rg = (RadioGroup) findViewById(R.id.radiogroup);
                int radioBtnChecked = rg.getCheckedRadioButtonId();
                RadioButton rb = (RadioButton) rg.findViewById(radioBtnChecked);
                if (radioBtnChecked < 0) {
                    radioText = "Nu ați selectat niciun candidat!";
                    Toast.makeText(VoteActivity.this, radioText, Toast.LENGTH_SHORT).show();
                }
                else {
                    radioText = rb.getText().toString();
                    String result[] = radioText.split("\n", 2);
                    String nameTxt, partidTxt;
                    nameTxt = result[0].substring(16);
                    partidTxt = result[1].substring(9);
                    post(nameTxt, partidTxt);
                }
            }

        });

        btnLogout.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent i = new Intent(VoteActivity.this, MainActivity.class);
                startActivity(i);
                Toast.makeText(VoteActivity.this, "Succefully Logout!", Toast.LENGTH_SHORT).show();
            }
        });
    }

    public void post(String nc, String np) {
        JSONObject js = new JSONObject();
        try {

            js.put("CNP", mCnpText);
            js.put("Partid", np);
            js.put("NumeCandidat", nc);
        } catch (JSONException e) {
            e.printStackTrace();
        }

        String url = "https://0cxr9anbqk.execute-api.eu-central-1.amazonaws.com/dev/insert-voter";
        JsonObjectRequest jsonObjectRequest = new JsonObjectRequest(Request.Method.POST,
                url, js,
                new Response.Listener<JSONObject>() {
                    @Override
                    public void onResponse(JSONObject response) {
                        Toast.makeText(VoteActivity.this, "Ați votat! Felicitări!", Toast.LENGTH_SHORT).show();
                        Intent i = new Intent(VoteActivity.this, MainActivity.class);
                        startActivity(i);
                    }
                },
                new Response.ErrorListener() {
                    @Override
                    public void onErrorResponse(VolleyError error) {
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

    @RequiresApi(api = Build.VERSION_CODES.LOLLIPOP)
    public void addRadioButtons() {
        RadioGroup rg = (RadioGroup) findViewById(R.id.radiogroup);
        rg.setOrientation(LinearLayout.VERTICAL);
        rg.setShowDividers(LinearLayout.SHOW_DIVIDER_MIDDLE);
        rg.setDividerDrawable(getResources().getDrawable(android.R.drawable.divider_horizontal_textfield, this.getTheme()));
        for (int j = 0; j < name.length; j++) {
            RadioButton rdbtn = new RadioButton(this);
            rdbtn.setPadding(10, 10, 10, 30);
            rdbtn.setId(j);
            rdbtn.setText("Nume Candidat:  " + name[j] + "\n" + "Partid:  " + partid[j]);
            rdbtn.setTextSize(20f);
            rg.addView(rdbtn);
        }
    }
}

