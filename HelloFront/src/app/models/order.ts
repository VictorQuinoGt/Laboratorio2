export interface OrderDetailCreateDto {
  itemId: number;
  quantity: number;
  price?: number;
}
export interface OrderCreateDto {
  personId: number;
  lines: OrderDetailCreateDto[]; // Cambia a 'details' si tu backend lo espera así
}
export interface OrderLineReadDto {
  id: number;
  itemId: number;
  itemName: string;
  quantity: number;
  price: number;
  total: number;
}
export interface OrderReadDto {
  id: number;
  number: number;
  personId: number;
  personName: string;
  createdAt: string;
  lines: OrderLineReadDto[];
  grandTotal: number;
}
