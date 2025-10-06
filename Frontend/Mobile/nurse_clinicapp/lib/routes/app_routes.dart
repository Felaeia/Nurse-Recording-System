import 'package:flutter/material.dart';
import 'package:nurse_clinicapp/landing/welcome.dart';
import '../screens/home.dart';
import '../screens/add_form.dart';
import '../screens/patients_record.dart';


class AppRoutes {
  static const String home = '/';
  static const String addForm = '/addform';
  static const String patientsRecords = '/patientsrecords';
  static const String appointments = '/appointments';
  static const String inventory = '/inventory';
  static const String userInfo = '/userinfo';
  static const String welcome = '/welcome';

  static Map<String, WidgetBuilder> getRoutes() {
    return {
      home: (context) => const Home(),
      addForm: (context) => const AddForm(),
      patientsRecords: (context) => const PatientRecords(),
      appointments: (context) => const Scaffold(), 
      inventory: (context) => const Scaffold(),
      userInfo: (context) => const Scaffold(),
      welcome: (context) => const Welcome(),
    };
  }
}
