import 'package:flutter/material.dart';
import '../widgets/app_colors.dart';

class MedicalInventory extends StatefulWidget {
  const MedicalInventory({super.key});

  @override
  State<MedicalInventory> createState() => _MedicalInventoryState();
}

class _MedicalInventoryState extends State<MedicalInventory> {
  List<Map<String, String>> medicines = [
    {'name': 'Paracetamol', 'category': 'Pain Reliever', 'quantity': '120', 'expiry': '12/2026'},
    {'name': 'Amoxicillin', 'category': 'Antibiotic', 'quantity': '85', 'expiry': '05/2027'},
    {'name': 'Cetirizine', 'category': 'Antihistamine', 'quantity': '150', 'expiry': '09/2026'},
    {'name': 'Ibuprofen', 'category': 'Anti-inflammatory', 'quantity': '22', 'expiry': '03/2027'},
    {'name': 'Insulin', 'category': 'Hormone', 'quantity': '8', 'expiry': '11/2025'},
  ];

  Color _getQuantityColor(String quantityStr) {
    final quantity = int.tryParse(quantityStr) ?? 0;
    if (quantity < 10) return Colors.red.shade700;
    if (quantity < 50) return Colors.orange.shade700;
    return Colors.green.shade700;
  }

  bool _isExpiringSoon(String expiry) {
    if (expiry.length < 7) return false;
    final year = int.tryParse(expiry.substring(3));
    return year != null && year <= 2025;
  }

  void _showMedicineDialog({Map<String, String>? medicine, int? index}) {
    final nameController = TextEditingController(text: medicine?['name'] ?? '');
    final categoryController = TextEditingController(text: medicine?['category'] ?? '');
    final quantityController = TextEditingController(text: medicine?['quantity'] ?? '');
    final expiryController = TextEditingController(text: medicine?['expiry'] ?? '');
    final _formKey = GlobalKey<FormState>();

    showDialog(
      context: context,
      builder: (context) {
        return Dialog(
          backgroundColor: Colors.transparent,
          insetPadding: const EdgeInsets.symmetric(horizontal: 24, vertical: 20),
          child: Container(
            decoration: BoxDecoration(
              gradient: AppColors.primaryGradient,
              borderRadius: BorderRadius.circular(20),
            ),
            padding: const EdgeInsets.all(3),
            child: Container(
              padding: const EdgeInsets.all(24),
              decoration: BoxDecoration(
                color: Colors.white,
                borderRadius: BorderRadius.circular(16),
              ),
              child: Form(
                key: _formKey,
                child: Column(
                  mainAxisSize: MainAxisSize.min,
                  children: [
                    Text(
                      medicine == null ? "Add Medicine" : "Edit Medicine",
                      style: TextStyle(
                        fontSize: 22,
                        fontWeight: FontWeight.bold,
                        color: AppColors.primaryGradient.colors.first,
                      ),
                    ),
                    const SizedBox(height: 20),
                    _buildGradientInput(nameController, "Medicine Name"),
                    const SizedBox(height: 16),
                    _buildGradientInput(categoryController, "Category"),
                    const SizedBox(height: 16),
                    _buildGradientInput(quantityController, "Quantity", keyboardType: TextInputType.number),
                    const SizedBox(height: 16),
                    _buildGradientInput(expiryController, "Expiry (MM/YYYY)"),
                    const SizedBox(height: 24),
                    Row(
                      children: [
                        Expanded(
                          child: ElevatedButton(
                            onPressed: () {
                              final newMedicine = {
                                'name': nameController.text,
                                'category': categoryController.text,
                                'quantity': quantityController.text,
                                'expiry': expiryController.text,
                              };
                              setState(() {
                                if (index != null) {
                                  medicines[index] = newMedicine;
                                } else {
                                  medicines.add(newMedicine);
                                }
                              });
                              Navigator.pop(context);
                            },
                            style: ElevatedButton.styleFrom(
                              backgroundColor: AppColors.primaryGradient.colors.first,
                              padding: const EdgeInsets.symmetric(vertical: 16),
                              shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
                            ),
                            child: const Text(
                              "Save",
                              style: TextStyle(fontWeight: FontWeight.bold, fontSize: 16, color: Colors.white),
                            ),
                          ),
                        ),
                        const SizedBox(width: 12),
                        Expanded(
                          child: MouseRegion(
                            cursor: SystemMouseCursors.click,
                            child: ElevatedButton(
                              onPressed: () => Navigator.pop(context),
                              style: ButtonStyle(
                                backgroundColor: MaterialStateProperty.resolveWith((states) {
                                  if (states.contains(MaterialState.hovered)) {
                                    return Colors.red.shade700;
                                  }
                                  return Colors.grey.shade300;
                                }),
                                foregroundColor: MaterialStateProperty.all(Colors.white),
                                padding: MaterialStateProperty.all(const EdgeInsets.symmetric(vertical: 16)),
                                shape: MaterialStateProperty.all(
                                  RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
                                ),
                              ),
                              child: const Text(
                                "Cancel",
                                style: TextStyle(fontWeight: FontWeight.bold, fontSize: 16),
                              ),
                            ),
                          ),
                        ),
                      ],
                    )
                  ],
                ),
              ),
            ),
          ),
        );
      },
    );
  }

  Widget _buildGradientInput(TextEditingController controller, String placeholder,
      {TextInputType keyboardType = TextInputType.text}) {
    return Container(
      decoration: BoxDecoration(
        gradient: AppColors.primaryGradient,
        borderRadius: BorderRadius.circular(14),
      ),
      padding: const EdgeInsets.all(2),
      child: TextFormField(
        controller: controller,
        keyboardType: keyboardType,
        style: const TextStyle(color: Colors.black87),
        decoration: InputDecoration(
          hintText: placeholder,
          hintStyle: TextStyle(color: AppColors.primaryGradient.colors.first.withOpacity(0.6)),
          filled: true,
          fillColor: Colors.white,
          border: OutlineInputBorder(borderRadius: BorderRadius.circular(12), borderSide: BorderSide.none),
          contentPadding: const EdgeInsets.symmetric(horizontal: 16, vertical: 14),
        ),
      ),
    );
  }

  Widget _buildMedicineCard(Map<String, String> med, int index) {
    final quantityColor = _getQuantityColor(med['quantity']!);
    final isExpiring = _isExpiringSoon(med['expiry']!);

    return Container(
      margin: const EdgeInsets.only(bottom: 16),
      decoration: BoxDecoration(
        gradient: AppColors.primaryGradient,
        borderRadius: BorderRadius.circular(18),
      ),
      child: Container(
        decoration: BoxDecoration(
          color: Colors.white,
          borderRadius: BorderRadius.circular(16),
        ),
        child: InkWell(
          borderRadius: BorderRadius.circular(16),
          onTap: () => _showMedicineDialog(medicine: med, index: index),
          child: Padding(
            padding: const EdgeInsets.all(20),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  med['name']!,
                  style: TextStyle(
                    fontSize: 20,
                    fontWeight: FontWeight.bold,
                    color: AppColors.primaryGradient.colors.first,
                  ),
                ),
                const SizedBox(height: 6),
                Text("Category: ${med['category']}", style: const TextStyle(color: Colors.black54, fontSize: 15)),
                const SizedBox(height: 4),
                Text(
                  "Quantity: ${med['quantity']}",
                  style: TextStyle(color: quantityColor, fontWeight: FontWeight.bold, fontSize: 16),
                ),
                const SizedBox(height: 4),
                Text(
                  "Expiry: ${med['expiry']}${isExpiring ? ' (Expiring!)' : ''}",
                  style: TextStyle(color: isExpiring ? Colors.red.shade700 : Colors.black87, fontWeight: FontWeight.bold, fontSize: 16),
                ),
                const SizedBox(height: 12),
                Row(
                  mainAxisAlignment: MainAxisAlignment.end,
                  children: [
                    TextButton(
                      onPressed: () => _showMedicineDialog(medicine: med, index: index),
                      child: Text(
                        "Edit",
                        style: TextStyle(color: AppColors.primaryGradient.colors.first, fontWeight: FontWeight.bold),
                      ),
                    ),
                    const SizedBox(width: 12),
                    TextButton(
                      onPressed: () => _deleteMedicine(index),
                      child: const Text(
                        "Delete",
                        style: TextStyle(color: Colors.redAccent, fontWeight: FontWeight.bold),
                      ),
                    ),
                  ],
                )
              ],
            ),
          ),
        ),
      ),
    );
  }

  void _deleteMedicine(int index) {
    setState(() {
      medicines.removeAt(index);
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: const Color(0xFFF0F4F8),
      appBar: AppBar(
        automaticallyImplyLeading: false,
        title: const Text("Medical Inventory", style: TextStyle(fontWeight: FontWeight.bold, fontSize: 24)),
        flexibleSpace: Container(decoration: const BoxDecoration(gradient: AppColors.primaryGradient)),
        elevation: 4,
        foregroundColor: Colors.white,
      ),
      body: Padding(
        padding: const EdgeInsets.all(20),
        child: medicines.isEmpty
            ? Center(
                child: Column(
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: [
                    Icon(Icons.inventory_2_rounded, size: 80, color: Colors.grey.shade400),
                    const SizedBox(height: 16),
                    const Text("Your inventory is empty.", style: TextStyle(fontSize: 18, color: Colors.black54)),
                    const Text("Tap the '+' button to add an item.", style: TextStyle(fontSize: 14, color: Colors.black45)),
                  ],
                ),
              )
            : ListView.builder(
                itemCount: medicines.length,
                itemBuilder: (context, index) => _buildMedicineCard(medicines[index], index),
              ),
      ),
      floatingActionButton: FloatingActionButton.extended(
        onPressed: () => _showMedicineDialog(),
        backgroundColor: AppColors.primaryGradient.colors.first,
        elevation: 8,
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(16)),
        icon: const Icon(Icons.add_rounded, color: Colors.white, size: 28),
        label: const Text("Add Medicine", style: TextStyle(color: Colors.white, fontWeight: FontWeight.bold)),
      ),
    );
  }
}
