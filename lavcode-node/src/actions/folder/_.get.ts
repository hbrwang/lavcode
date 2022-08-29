import { Inject } from "@ipare/inject";
import { Action } from "@ipare/router";
import {
  ApiDescription,
  ApiResponses,
  ApiSecurity,
  ApiTags,
} from "@ipare/swagger";
import { FolderEntity } from "../../entities/folder.entity";
import { IconEntity } from "../../entities/icon.entity";
import { CbappService } from "../../services/cbapp.service";
import { CollectionService } from "../../services/collection.service";
import { GetFolderDto } from "./dtos/get-folder.dto";

@ApiTags("folder")
@ApiDescription("Get all folders")
@ApiResponses({
  "200": {
    description: "success",
  },
})
@ApiSecurity({
  Bearer: [],
})
export default class extends Action {
  @Inject
  private readonly collectionService!: CollectionService;
  @Inject
  private readonly cbappService!: CbappService;

  async invoke() {
    const $ = this.cbappService.db.command.aggregate;

    const res = await this.collectionService.folder
      .aggregate()
      .lookup({
        from: this.collectionService.icon.name,
        localField: "_id",
        foreignField: "_id",
        as: "icons",
      })
      .addFields({
        icon: $.arrayElemAt(["$icons", 0]),
      })
      .project({
        icons: 0,
      })
      .end();
    const folderEntities = res.data as (FolderEntity & {
      icon: IconEntity;
    })[];
    const folders = folderEntities.map((f) =>
      GetFolderDto.fromEntity(f, f.icon)
    );
    this.ok(folders);
  }
}
