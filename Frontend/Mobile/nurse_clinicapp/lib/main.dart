import 'package:flutter/material.dart';
import 'package:device_preview/device_preview.dart';
import 'package:nurse_clinicapp/landing/welcome.dart';

void main() {
  runApp(DevicePreview(enabled: true, builder: (context) => ClinicApp()));
}

class ClinicApp extends StatelessWidget {
  const ClinicApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      locale: DevicePreview.locale(context),
      builder: DevicePreview.appBuilder,
      debugShowCheckedModeBanner: false,
      theme: ThemeData(
        fontFamily: 'Poppins',
      ),
      home: Welcome(),
    );
  }
}
