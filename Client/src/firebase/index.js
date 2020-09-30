import firebase from 'firebase';

var firebaseConfig = {
    apiKey: "AIzaSyBvGw0ZeVv6Jfa-DXESjkJv90jgiUp3nr4",
    authDomain: "todoist-774e1.firebaseapp.com",
    databaseURL: "https://todoist-774e1.firebaseio.com",
    projectId: "todoist-774e1",
    storageBucket: "todoist-774e1.appspot.com",
    messagingSenderId: "11121563154",
    appId: "1:11121563154:web:773b0f7e9c581268b7eacb",
    measurementId: "G-LCGEJ7JRZ7"
  };
// Initialize Firebase
firebase.initializeApp(firebaseConfig);
firebase.analytics();

export {
    firebase
}

