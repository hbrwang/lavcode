import { DtoRequired } from "@ipare/swagger";
import { IsNotEmpty, IsString } from "class-validator";
import { UpsertIconDto } from "../../icon/dtos/upsert-icon.dto";

export class CreateFolderDto {
  @DtoRequired()
  @IsString()
  name!: string;

  @DtoRequired()
  @IsNotEmpty()
  icon!: UpsertIconDto;
}
